using UniRx;

public sealed class ScoreDisplayPresenter : VContainer.Unity.IInitializable {
	private readonly ScoreManager m_ScoreManager;
	private readonly ReactiveReferee m_ReactiveReferee;
	private readonly ScoreDisplay m_ScoreDisplay;

	public ScoreDisplayPresenter (ReactiveReferee _reactiveReferee, ScoreDisplay _scoreDisplay, ScoreManager _scoreManager) {
		m_ScoreManager = _scoreManager;
		m_ReactiveReferee = _reactiveReferee;
		m_ScoreDisplay = _scoreDisplay;
	}

	public void Initialize() {
		m_ReactiveReferee.OnHit.Subscribe (x => {
			m_ScoreManager.AddScore (100);
		});

		m_ScoreManager.m_CurrentScore.Subscribe (x => {
			m_ScoreDisplay.SetScore (x);
		});
	}
}
