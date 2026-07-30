using UniRx;

public sealed class LaneVisualizingPresenter : VContainer.Unity.IStartable, System.IDisposable {
	private readonly InputHandlerFactory m_InputHandlerFactory;
	private readonly InputVisualizer m_Visualizer;

	private readonly CompositeDisposable m_Disposables = new();

	[VContainer.Inject]
	public LaneVisualizingPresenter (InputHandlerFactory _inputHandler, InputVisualizer _visualizer) {
		m_InputHandlerFactory = _inputHandler;
		m_Visualizer = _visualizer;
	}

	public void Start() {
		m_InputHandlerFactory.HandlerPool.OnPressed.Subscribe (_ => {
			m_Visualizer.Activate (_);
		}).AddTo (m_Disposables);

		m_InputHandlerFactory.HandlerPool.OnReleased.Subscribe (_ => {
			m_Visualizer.Deactivate (_);
		}).AddTo (m_Disposables);
	}

	public void Dispose() {
		m_Disposables.Dispose();
	}
}
