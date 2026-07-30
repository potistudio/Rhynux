using MackySoft.Navigathena.SceneManagement;

namespace Rhynux.Title {
	public sealed class SceneMover : UnityEngine.MonoBehaviour {
		private ISceneIdentifier m_SceneIdentifier;

		private void Awake() {
			m_SceneIdentifier = new BuiltInSceneIdentifier ("SongSelectionMenu");
		}

		// Stays a plain void so UnityEvent (button OnClick) can still bind to it.
		public void NextScene() {
			PushNextSceneAsync().Forget();
		}

		// UniTaskVoid over async void: an exception thrown inside async void is swallowed
		// by the synchronization context, while Forget() routes it to UniTask's handler.
		private async Cysharp.Threading.Tasks.UniTaskVoid PushNextSceneAsync() {
			await GlobalSceneNavigator.Instance.Push (m_SceneIdentifier);
		}
	}
}
