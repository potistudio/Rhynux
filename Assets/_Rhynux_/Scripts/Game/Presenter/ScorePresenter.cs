using UniRx;

public sealed class ScorePresenter : VContainer.Unity.IInitializable {
	private readonly NotesRefereeComposer m_NotesReferee;
	private readonly ScoreManager m_ScoreManager;

	private readonly int m_NotesCount;
	private readonly float m_DeltaScore;

	public ScorePresenter (SessionData _session, NotesRefereeComposer _notesReferee, ScoreManager _scoreManager) {
		m_NotesReferee = _notesReferee;
		m_ScoreManager = _scoreManager;

		m_NotesCount = _session.Notes.Length;
		m_DeltaScore = 1000000f / m_NotesCount;
	}

	public void Initialize() {
		m_NotesReferee.OnHit.Subscribe (x => {
			m_ScoreManager.AddScore (m_DeltaScore);
		});
	}
}
