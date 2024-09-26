using UnityEngine;

public sealed class SceneNavigationInput : MonoBehaviour {
	private SceneNavigator m_SceneNavigator;

	[VContainer.Inject]
	private void Inject (SceneNavigator _sceneNavigator) {
		m_SceneNavigator = _sceneNavigator;
	}

	private void Update() {
		if (Input.GetKeyDown(KeyCode.Escape))
			m_SceneNavigator.StartSession();
	}
}
