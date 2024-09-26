using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MackySoft.Navigathena.SceneManagement;

public class SceneNavigator : MonoBehaviour {
	private ScrollView m_ScrollView;

	[VContainer.Inject]
	private void Init (ScrollView _view) {
		m_ScrollView = _view;
	}

	public async void StartSession() {
		ISceneIdentifier sceneIdentifier = new BuiltInSceneIdentifier ("Sample");
		int index = m_ScrollView.CurrentSelectingIndex;
		Chart selectingChart = m_ScrollView.CurrentSelectingChart;

		await GlobalSceneNavigator.Instance.Push (sceneIdentifier, null, new GameSceneRequest { AutoMode = true, Chart = selectingChart });
	}
}
