using System.Threading;
using UnityEngine;
using Cysharp.Threading.Tasks;
using MackySoft.Navigathena.SceneManagement;
using MackySoft.Navigathena.SceneManagement.VContainer;

public sealed class SceneEntryPoint : SceneLifecycleBase {
	private SessionFactory m_Factory;

	[VContainer.Inject]
	private void Inject (SessionFactory _factory) {
		m_Factory = _factory;
	}

	protected override UniTask OnInitialize(ISceneDataReader reader, System.IProgress<MackySoft.Navigathena.IProgressDataStore> progress, CancellationToken cancellationToken) {
		Debug.Log ("Scene Initialized");
		Debug.Log (m_Factory);

		return base.OnInitialize (reader, progress, cancellationToken);
	}

	protected override UniTask OnEnter (ISceneDataReader reader, CancellationToken cancellationToken) {
		Debug.Log ("Scene Entered");

		return base.OnEnter (reader, cancellationToken);
	}
}
