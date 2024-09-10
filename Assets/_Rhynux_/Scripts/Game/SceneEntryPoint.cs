using System.Threading;
using UnityEngine;
using Cysharp.Threading.Tasks;
using MackySoft.Navigathena.SceneManagement;
using MackySoft.Navigathena.SceneManagement.VContainer;

public sealed class SceneEntryPoint : SceneLifecycleBase {
	[SerializeReference] public INotesGenerator m_NotesGenerator;
	[SerializeField] private MusicPlayer m_MusicPlayer;
	[SerializeField] private Chart m_Chart;
	[SerializeField] private _FullLogic m_FullLogic;

	protected override UniTask OnInitialize(ISceneDataReader reader, System.IProgress<MackySoft.Navigathena.IProgressDataStore> progress, CancellationToken cancellationToken) {
		Debug.Log ("Scene Initialized");

		return base.OnInitialize (reader, progress, cancellationToken);
	}

	protected override UniTask OnEnter (ISceneDataReader reader, CancellationToken cancellationToken) {
		Debug.Log ("Scene Entered");

		return base.OnEnter (reader, cancellationToken);
	}
}
