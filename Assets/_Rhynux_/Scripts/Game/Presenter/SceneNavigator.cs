using Cysharp.Threading.Tasks;
using MackySoft.Navigathena.SceneManagement;

public sealed class SceneNavigator {
	public void StartSession() {
		PopAsync().Forget();
	}

	// UniTaskVoid over async void: an exception thrown inside async void is swallowed
	// by the synchronization context, while Forget() routes it to UniTask's handler.
	private async UniTaskVoid PopAsync() {
		await GlobalSceneNavigator.Instance.Pop();
	}
}
