using UniRx;

public sealed class ScoreDisplayPresenter : VContainer.Unity.IInitializable {
	private readonly ScoreManager m_ScoreManager;
	private readonly ScoreDisplay m_ScoreDisplay;

	public ScoreDisplayPresenter (ScoreDisplay _scoreDisplay, ScoreManager _scoreManager) {
		m_ScoreManager = _scoreManager;
		m_ScoreDisplay = _scoreDisplay;
	}

	public void Initialize() {
		m_ScoreManager.m_CurrentScore.Subscribe (x => {
			m_ScoreDisplay.SetScore (UnityEngine.Mathf.RoundToInt(x));
		});
	}
}
