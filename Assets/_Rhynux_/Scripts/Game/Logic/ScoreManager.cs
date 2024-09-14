public class ScoreManager {
	public readonly UniRx.ReactiveProperty<int> m_CurrentScore = new (0);

	public void AddScore (int _deltaScore) {
		m_CurrentScore.Value += _deltaScore;
	}
}
