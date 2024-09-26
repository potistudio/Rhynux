public sealed class LogicPresenter : VContainer.Unity.IStartable {
	private readonly _FullLogic m_Logic;

	[VContainer.Inject]
	public LogicPresenter (_FullLogic _logic) {
		m_Logic = _logic;
	}

	public void Start() {
		m_Logic.Init();
	}
}
