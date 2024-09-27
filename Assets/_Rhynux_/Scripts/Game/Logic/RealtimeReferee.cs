using System.Linq;

public sealed class RealtimeReferee {
	private readonly UniRx.Subject<(int, NoteAvailableStatus)> m_NoteStatusChanged = new();
	public System.IObservable<(int, NoteAvailableStatus)> OnNoteStatusChanged => m_NoteStatusChanged;

	private readonly float m_Margin = 0.160f;

	private float m_CurrentTime = 0f;
	private readonly SessionFactory m_SessionFactory;

	public RealtimeReferee (SessionFactory _session) {
		m_SessionFactory = _session;
	}

	public void UpdateTime (float _time) {
		Note[] notes = m_SessionFactory.SessionPool.Notes;
		int newIndex = FindBehindNote (_time);

		var targets = notes.Where ((x, i) => i <= newIndex).Select ((x, i) => (x, i));
		foreach ((Note x, int i) in targets) {
			FallNote (i);
		}

		m_CurrentTime = _time;
	}

	private int FindBehindNote (float _time) {
		Note[] notes = m_SessionFactory.SessionPool.Notes;
		var n = notes.Where (x => x.Time + m_Margin < _time);
		if (n.Count() == 0)
			return -1;

		return n.Select((x, i) => i).LastOrDefault();
	}

	private void FallNote (int _targetIndex) {
		m_NoteStatusChanged.OnNext ((_targetIndex, NoteAvailableStatus.Fell));
	}
}
