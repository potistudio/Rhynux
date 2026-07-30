using UniRx;

public sealed class ComboPresenter : VContainer.Unity.IInitializable, System.IDisposable {
	private readonly RefereeFacade m_Referee;
	private readonly ComboManager m_ComboManager;

	private readonly CompositeDisposable m_Disposables = new();

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
		}).AddTo (m_Disposables);

		m_Referee.OnFall.Subscribe (x => {
			m_ComboManager.ResetCombo();
		}).AddTo (m_Disposables);
	}

	public void Dispose() {
		m_Disposables.Dispose();
	}
}
