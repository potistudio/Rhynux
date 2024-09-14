using UniRx;

public class ComboPresenter : VContainer.Unity.IInitializable {
	private readonly ComboManager m_ComboManager;
	private readonly ReactiveReferee m_Referee;

	public ComboPresenter (ReactiveReferee _referee, ComboManager _comboManager) {
		m_Referee = _referee;
		m_ComboManager = _comboManager;
	}

	public void Initialize() {
		m_Referee.OnHit.Subscribe (x => {
			if (x.Item3 == AccuracyLevel.Miss) {
				m_ComboManager.ResetCombo();
				return;
			}

			m_ComboManager.IncreaseCombo();
		});
	}
}
