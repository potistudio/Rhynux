using UniRx;

public sealed class ComboPresenter : VContainer.Unity.IInitializable {
	private readonly RefereeFacade m_Referee;
	private readonly ComboManager m_ComboManager;

	public ComboPresenter (RefereeFacade _referee, ComboManager _comboManager) {
		m_Referee = _referee;
		m_ComboManager = _comboManager;
	}

	public void Initialize() {
		m_Referee.OnHit.Subscribe (x => {
			if (x.accuracy == AccuracyLevel.Miss) {
				m_ComboManager.ResetCombo();
				return;
			}

			m_ComboManager.IncreaseCombo();
		});

		m_Referee.OnFall.Subscribe (x => {
			m_ComboManager.ResetCombo();
		});
	}
}
