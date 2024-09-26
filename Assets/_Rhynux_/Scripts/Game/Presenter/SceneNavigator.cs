using MackySoft.Navigathena.SceneManagement;

public sealed class SceneNavigator {
	public async void StartSession() {
		await GlobalSceneNavigator.Instance.Pop();
	}
}
