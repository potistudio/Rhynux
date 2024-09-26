using System.Threading;
using UnityEngine;
using Cysharp.Threading.Tasks;
using MackySoft.Navigathena.SceneManagement;
using MackySoft.Navigathena.SceneManagement.VContainer;

public sealed class SceneEntryPoint : SceneLifecycleBase {
	private SessionFactory m_Factory;
	private Chart m_Chart;

	[VContainer.Inject]
	private void Inject (SessionFactory _factory, Chart _chart = null) {
		m_Factory = _factory;
		m_Chart = _chart;
	}

	protected override UniTask OnInitialize(ISceneDataReader reader, System.IProgress<MackySoft.Navigathena.IProgressDataStore> progress, CancellationToken cancellationToken) {
		Debug.Log ("Scene Initialized");
		Debug.Log (m_Factory);

		if (reader.TryRead(out GameSceneRequest data)) {
			Debug.Log ("Read Chart " + data.AutoMode);

			// Generate Session
			SessionData session = m_Factory.Create (data.Chart);
			Debug.Log (session.Notes.Length);
		} else if (m_Chart != null) {
			SessionData session = m_Factory.Create (m_Chart);
			Debug.Log (session.Notes.Length);
		} else {
			throw new System.OperationCanceledException();
		}

		return base.OnInitialize (reader, progress, cancellationToken);
	}

	protected override UniTask OnEnter (ISceneDataReader reader, CancellationToken cancellationToken) {
		Debug.Log ("Scene Entered");

		return base.OnEnter (reader, cancellationToken);
	}
}
