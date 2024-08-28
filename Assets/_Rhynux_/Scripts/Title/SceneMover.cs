using MackySoft.Navigathena.SceneManagement;

public sealed class SceneMover : UnityEngine.MonoBehaviour {
	private ISceneIdentifier m_SceneIdentifier;

	private void Awake() {
		m_SceneIdentifier = new BuiltInSceneIdentifier ("SongSelectionMenu");
	}

	public async void NextScene() {
		await GlobalSceneNavigator.Instance.Push (m_SceneIdentifier);
	}
}
