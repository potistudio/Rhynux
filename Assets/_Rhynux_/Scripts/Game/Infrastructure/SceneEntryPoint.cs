using System.Threading;
using UnityEngine;
using Cysharp.Threading.Tasks;
using MackySoft.Navigathena.SceneManagement;
using MackySoft.Navigathena.SceneManagement.VContainer;

public sealed class SceneEntryPoint : SceneLifecycleBase {
	private SessionFactory m_Factory;
	private SessionProxy m_SessionProxy;
	private InputHandlerFactory m_InputFactory;
	private Chart m_Chart;

	[VContainer.Inject]
	private void Inject (SessionFactory _factory, SessionProxy _sessionProxy, InputHandlerFactory _inputFactory, Chart _chart = null) {
		m_Factory = _factory;
		m_SessionProxy = _sessionProxy;
		m_InputFactory = _inputFactory;
		m_Chart = _chart;
	}

	protected override UniTask OnInitialize(ISceneDataReader reader, System.IProgress<MackySoft.Navigathena.IProgressDataStore> progress, CancellationToken cancellationToken) {
		SessionData session;
		IInputHandler inputHandler;

		if (reader.TryRead(out GameSceneRequest data)) {  // Read session request if available
			Debug.Log ("Read Chart " + data.AutoMode);
			session = m_Factory.Create (data.Chart);
		} else if (m_Chart != null) {  // Read chart if session request is not available
			session = m_Factory.Create (m_Chart);
		} else {  // throw error if there is no chart
			throw new System.OperationCanceledException();
		}

		inputHandler = m_InputFactory.Create (InputMode.Auto);

		m_SessionProxy.Session = session; // Set session

		Debug.Log ("Scene Initialized");
		return base.OnInitialize (reader, progress, cancellationToken);
	}

	protected override UniTask OnEnter (ISceneDataReader reader, CancellationToken cancellationToken) {
		Debug.Log ("Scene Entered");

		return base.OnEnter (reader, cancellationToken);
	}
}
