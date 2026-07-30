using UniRx;

public sealed class ComboDisplayPresenter : VContainer.Unity.IInitializable, System.IDisposable {
	private readonly ComboDisplay m_View;
	private readonly ComboManager m_ComboManager;

	private readonly CompositeDisposable m_Disposables = new();

	public ComboDisplayPresenter (ComboDisplay _view, ComboManager _comboManager) {
		m_ComboManager = _comboManager;
		m_View = _view;
	}

	public void Initialize() {
		m_View.SetValue (0);

		m_ComboManager.m_CurrentCombo.Subscribe (x => {
			m_View.SetValue (x);
		}).AddTo (m_Disposables);
	}

	public void Dispose() {
		m_Disposables.Dispose();
	}
}
