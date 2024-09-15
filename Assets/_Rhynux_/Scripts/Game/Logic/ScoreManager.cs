public class ScoreManager {
	public readonly UniRx.ReactiveProperty<float> m_CurrentScore = new (0f);

	public void AddScore (float _deltaScore) {
		m_CurrentScore.Value += _deltaScore;
	}
}
