public class ComboManager {
	public readonly UniRx.ReactiveProperty<int> m_CurrentCombo = new (0);

	public void ResetCombo() {
		m_CurrentCombo.Value = 0;
	}

	public void IncreaseCombo() {
		m_CurrentCombo.Value++;
	}
}
