
using UniRx;

public class NotesReferee {
	#region Private Member
		private readonly SessionManager m_SessionManager;
		private readonly RealtimeReferee m_RealtimeReferee;
		private readonly ReactiveReferee m_ReactiveReferee;
	#endregion

	#region UniRx Events
		private readonly Subject<AccuracyLevel> m_OnHit = new ();
		public System.IObservable<AccuracyLevel> OnHit => m_OnHit;

		private readonly Subject<AccuracyLevel> m_OnFall = new ();
		public System.IObservable<AccuracyLevel> OnFall => m_OnFall;
	#endregion

	public NotesReferee (SessionManager _sessionManager, RealtimeReferee _realtimeReferee, ReactiveReferee _reactiveReferee) {
		m_SessionManager = _sessionManager;
		m_RealtimeReferee = _realtimeReferee;
		m_ReactiveReferee = _reactiveReferee;

		// When Session Time is Updated, Update the Realtime Referee
		m_SessionManager.OnTimeUpdated.Subscribe (x => {
			m_RealtimeReferee.UpdateTime (x);
			m_ReactiveReferee.UpdateTime (x);
		});

		// When Notes Updated by Realtime Referee, also Update the Notes in Session Manager
		m_RealtimeReferee.OnNoteStatusChanged.Subscribe (x => {
			// Fall Note only when It is Available
			if (m_SessionManager.NotesCollection[x.Item1].AvailableStatus != NoteAvailableStatus.Available)
				return;

			m_SessionManager.SetNoteStatus (x.Item1, x.Item2);
		});

		m_ReactiveReferee.OnHit.Subscribe (x => {
			if (x.Item2 == AccuracyLevel.Pass)
				return;

			if (m_SessionManager.NotesCollection[x.Item1].AvailableStatus == NoteAvailableStatus.Available)
				m_OnHit.OnNext (x.Item2);

			m_SessionManager.SetNoteStatus (x.Item1, NoteAvailableStatus.Hit);
		});
	}
}
