using UniRx;

namespace Rhynux.Game {
	public sealed class RefereeFacade : System.IDisposable {
		private readonly InputReferee m_InputReferee;
		private readonly RealtimeReferee m_RealtimeReferee;

		private readonly Subject<(int index, int lane, AccuracyLevel accuracy)> m_OnHit = new();
		public System.IObservable<(int index, int lane, AccuracyLevel accuracy)> OnHit => m_OnHit;

		private readonly Subject<Unit> m_OnFall = new();
		public System.IObservable<Unit> OnFall => m_OnFall;

		// Set rather than List: this is probed once per judged note, so a linear scan
		// would make the whole judging path quadratic in the note count.
		private readonly System.Collections.Generic.HashSet<int> m_CheckedNotes = new();

		private readonly CompositeDisposable m_Disposables = new();

		public RefereeFacade (SessionFactory _factory) {
			m_InputReferee = new (_factory);
			m_RealtimeReferee = new (_factory);

			m_InputReferee.OnHit.Subscribe (x => {
				if (!m_CheckedNotes.Add (x.index))
					return;

				m_OnHit.OnNext (x);
			}).AddTo (m_Disposables);

			m_RealtimeReferee.OnNoteStatusChanged.Subscribe (x => {
				if (!m_CheckedNotes.Add (x.Item1))
					return;

				m_OnFall.OnNext (Unit.Default);
			}).AddTo (m_Disposables);
		}

		public void UpdateTime (float _time) {
			m_RealtimeReferee.UpdateTime (_time);
			m_InputReferee.UpdateTime (_time);
		}

		public void Press (int _lane) {
			m_InputReferee.JudgeHit (_lane);
		}

		public void Release (int _lane) {
			// Reserved for hold notes; the input handlers already emit release events.
		}

		public void Dispose() {
			m_Disposables.Dispose();
			m_OnHit.Dispose();
			m_OnFall.Dispose();
		}
	}
}
