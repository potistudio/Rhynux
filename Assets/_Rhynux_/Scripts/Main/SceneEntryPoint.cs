using System.Threading;
using UnityEngine;
using Cysharp.Threading.Tasks;
using MackySoft.Navigathena.SceneManagement;

public sealed class SceneEntryPoint : SceneEntryPointBase {
	protected override UniTask OnEnter (ISceneDataReader reader, CancellationToken cancellationToken) {
		Debug.Log ("Enter");
		return base.OnEnter (reader, cancellationToken);
	}
}
