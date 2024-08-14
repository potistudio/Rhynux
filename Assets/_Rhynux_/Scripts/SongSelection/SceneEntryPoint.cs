using UnityEngine;
using VContainer;
using System.Threading;
using Cysharp.Threading.Tasks;
using MackySoft.Navigathena.SceneManagement;
using MackySoft.Navigathena.SceneManagement.VContainer;

public sealed class SceneEntryPoint : SceneLifecycleBase {
	protected override UniTask OnEnter (ISceneDataReader reader, CancellationToken cancellationToken) {
		Debug.Log ("Enter");

		return base.OnEnter (reader, cancellationToken);
	}
}
