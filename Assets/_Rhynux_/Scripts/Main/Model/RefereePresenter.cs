
using UniRx;

public class RefereePresenter {
	private readonly SessionManager m_SessionManager;
	private readonly RealtimeReferee m_RealtimeReferee;
	private readonly ReactiveReferee m_ReactiveReferee;

	public RefereePresenter (SessionManager _sessionManager, RealtimeReferee _realtimeReferee, ReactiveReferee _reactiveReferee) {
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
			m_SessionManager.SetNoteStatus (x.Item1, x.Item2);
		});

		m_ReactiveReferee.OnHit.Subscribe (x => {
			if (x.Item2 == AccuracyLevel.Pass)
				return;

			m_SessionManager.SetNoteStatus (x.Item1, NoteAvailableStatus.Hit);
		});
	}
}
