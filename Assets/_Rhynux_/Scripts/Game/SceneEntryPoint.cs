using System.Threading;
using UnityEngine;
using Cysharp.Threading.Tasks;
using MackySoft.Navigathena.SceneManagement;

public sealed class SceneEntryPoint : SceneEntryPointBase {
	[SerializeReference] public INotesGenerator m_NotesGenerator;
	[SerializeField] private MusicPlayer m_MusicPlayer;
	[SerializeField] private Chart m_Chart;

	protected override UniTask OnInitialize(ISceneDataReader reader, System.IProgress<MackySoft.Navigathena.IProgressDataStore> progress, CancellationToken cancellationToken) {
		Debug.Log ("Initialize");

		m_MusicPlayer.Clip = m_Chart.Clip;
		m_NotesGenerator.Generate (m_Chart);

		return base.OnInitialize (reader, progress, cancellationToken);
	}

	protected override UniTask OnEnter (ISceneDataReader reader, CancellationToken cancellationToken) {
		Debug.Log ("Enter");

		m_MusicPlayer.Play();

		return base.OnEnter (reader, cancellationToken);
	}
}
