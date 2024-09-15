using UniRx;

public class ComboPresenter : VContainer.Unity.IInitializable {
	private readonly ComboManager m_ComboManager;
	private readonly NotesRefereeComposer m_Referee;

	public ComboPresenter (NotesRefereeComposer _referee, ComboManager _comboManager) {
		m_Referee = _referee;
		m_ComboManager = _comboManager;
	}

	public void Initialize() {
		m_Referee.OnHit.Subscribe (x => {
			if (x == AccuracyLevel.Miss) {
				m_ComboManager.ResetCombo();
				return;
			}

			m_ComboManager.IncreaseCombo();
		});
	}
}
