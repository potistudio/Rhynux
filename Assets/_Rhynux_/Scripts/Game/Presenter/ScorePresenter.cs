using UniRx;

public sealed class ScorePresenter : VContainer.Unity.IInitializable, VContainer.Unity.IStartable {
	private readonly RefereeFacade m_NotesReferee;
	private readonly ScoreManager m_ScoreManager;
	private readonly SessionFactory m_Session;

	private int m_NotesCount;
	private float m_DeltaScore;

	public ScorePresenter (SessionFactory _session, RefereeFacade _notesReferee, ScoreManager _scoreManager) {
		m_Session = _session;
		m_NotesReferee = _notesReferee;
		m_ScoreManager = _scoreManager;
	}

	public void Initialize() {
		m_NotesReferee.OnHit.Subscribe (x => {
			m_ScoreManager.AddScore (m_DeltaScore);
		});
	}

	public void Start() {
		m_NotesCount = m_Session.SessionPool.Notes.Length;
		m_DeltaScore = 1000000f / m_NotesCount;
	}
}
