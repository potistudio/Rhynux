using UniRx;

public sealed class RefereeFacade {
	private readonly InputReferee m_InputReferee;
	private readonly RealtimeReferee m_RealtimeReferee;

	private UniRx.Subject<(int index, int lane, AccuracyLevel accuracy)> m_OnHit = new();
	public System.IObservable<(int index, int lane, AccuracyLevel accuracy)> OnHit => m_OnHit;

	private readonly Subject<Unit> m_OnFall = new();
	public System.IObservable<Unit> OnFall => m_OnFall;

	System.Collections.Generic.List<int> m_CheckedNotes = new();

	public RefereeFacade (SessionFactory _factory) {
		m_InputReferee = new (_factory);
		m_RealtimeReferee = new (_factory);

		m_InputReferee.OnHit.Subscribe (x => {
			if (m_CheckedNotes.Contains(x.Item1))
				return;

			m_OnHit.OnNext (x);
			m_CheckedNotes.Add (x.index);
		});

		m_RealtimeReferee.OnNoteStatusChanged.Subscribe (x => {
			if (m_CheckedNotes.Contains(x.Item1))
				return;

			m_OnFall.OnNext (Unit.Default);
			m_CheckedNotes.Add (x.Item1);
		});
	}

	public void UpdateTime (float _time) {
		m_RealtimeReferee.UpdateTime (_time);
		m_InputReferee.UpdateTime (_time);
	}

	public void Press (int _lane) {
		m_InputReferee.JudgeHit (_lane);
	}

	public void Release (int _lane) {
		// m_InputReferee.Release (_lane);
	}

	/*
	public InputReferee m_ReactiveReferee { get; set; }
	public RealtimeReferee m_RealtimeReferee { get; set; }
	*/
}
