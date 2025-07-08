using System.Threading;
using UnityEngine;
using Cysharp.Threading.Tasks;
using MackySoft.Navigathena.SceneManagement;
using MackySoft.Navigathena.SceneManagement.VContainer;

public sealed class SceneEntryPoint : SceneLifecycleBase {
	[SerializeField] private ChartAsset m_Chart;

	private SessionFactory m_SessionFactory;
	private SessionProxy m_SessionProxy;
	private InputHandlerFactory m_InputFactory;

	// [VContainer.Inject]
	// private void Inject (SessionFactory _factory, SessionProxy _sessionProxy, InputHandlerFactory _inputFactory, Chart _chart = null) {
	// 	m_SessionFactory = _factory;
	// 	m_SessionProxy = _sessionProxy;
	// 	m_InputFactory = _inputFactory;
	// 	m_Chart = _chart;
	// }

	protected override UniTask OnInitialize(ISceneDataReader reader, System.IProgress<MackySoft.Navigathena.IProgressDataStore> progress, CancellationToken cancellationToken) {
		// SessionData session;
		// IInputHandler inputHandler;

		if (reader.TryRead(out GameSceneRequest data)) {  // Read session request if available
			Debug.Log("[Rhynux] <DEBUG> - Read Chart " + data.AutoMode);
			// session = m_SessionFactory.Create(data.Chart);
		}
		// else if (m_Chart != null) {  // Read chart if session request is not available
		// 	session = m_SessionFactory.Create(m_Chart);
		// }
		// else {  // throw error if there is no chart
		// throw new System.OperationCanceledException();
		// }

		// inputHandler = m_InputFactory.Create(InputMode.Keyboard);

		// m_SessionProxy.Session = session; // Set session

		Debug.Log("[Rhynux] <INFO> - Scene Initialized");
		return base.OnInitialize(reader, progress, cancellationToken);
	}

	protected override UniTask OnEnter(ISceneDataReader reader, CancellationToken cancellationToken) {
		Debug.Log("[Rhynux] <INFO> - Scene Entered");
		return base.OnEnter(reader, cancellationToken);
	}

#if UNITY_EDITOR
	protected override UniTask OnEditorFirstPreInitialize(ISceneDataWriter writer, CancellationToken cancellationToken) {
		return base.OnEditorFirstPreInitialize(writer, cancellationToken);
	}
#endif
}
