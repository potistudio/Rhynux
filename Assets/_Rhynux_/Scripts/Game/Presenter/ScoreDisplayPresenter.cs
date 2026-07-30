using UniRx;

namespace Rhynux.Game {
	public sealed class ScoreDisplayPresenter : VContainer.Unity.IInitializable, System.IDisposable {
		private readonly ScoreManager m_ScoreManager;
		private readonly ScoreDisplay m_ScoreDisplay;

		private readonly CompositeDisposable m_Disposables = new();

		public ScoreDisplayPresenter (ScoreDisplay _scoreDisplay, ScoreManager _scoreManager) {
			m_ScoreManager = _scoreManager;
			m_ScoreDisplay = _scoreDisplay;
		}

		public void Initialize() {
			m_ScoreManager.m_CurrentScore.Subscribe (x => {
				m_ScoreDisplay.SetScore (UnityEngine.Mathf.RoundToInt(x));
			}).AddTo (m_Disposables);
		}

		public void Dispose() {
			m_Disposables.Dispose();
		}
	}
}
