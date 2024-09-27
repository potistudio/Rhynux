using UniRx;

public sealed class LaneVisualizingPresenter : VContainer.Unity.IStartable {
	private readonly InputHandlerFactory m_InputHandlerFactory;
	private readonly InputVisualizer m_Visualizer;

	[VContainer.Inject]
	public LaneVisualizingPresenter (InputHandlerFactory _inputHandler, InputVisualizer _visualizer) {
		m_InputHandlerFactory = _inputHandler;
		m_Visualizer = _visualizer;
	}

	public void Start() {
		m_InputHandlerFactory.HandlerPool.OnPressed.Subscribe (_ => {
			m_Visualizer.Activate (_);
		});

		m_InputHandlerFactory.HandlerPool.OnReleased.Subscribe (_ => {
			m_Visualizer.Deactivate (_);
		});
	}
}
