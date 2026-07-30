using Cysharp.Threading.Tasks;
using UnityEngine;
using MackySoft.Navigathena.SceneManagement;

namespace Rhynux.SongSelection {
	public class SceneNavigator : MonoBehaviour {
		private ScrollView m_ScrollView;

		[VContainer.Inject]
		private void Init (ScrollView _view) {
			m_ScrollView = _view;
		}

		public void StartSession() {
			PushSessionAsync().Forget();
		}

		// UniTaskVoid over async void: an exception thrown inside async void is swallowed
		// by the synchronization context, while Forget() routes it to UniTask's handler.
		private async UniTaskVoid PushSessionAsync() {
			ISceneIdentifier sceneIdentifier = new BuiltInSceneIdentifier ("Sample");
			Chart selectingChart = m_ScrollView.CurrentSelectingChart;

			await GlobalSceneNavigator.Instance.Push (sceneIdentifier, null, new GameSceneRequest { AutoMode = true, Chart = selectingChart });
		}
	}
}
