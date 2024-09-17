using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MackySoft.Navigathena.SceneManagement;

public class SceneNavigator : MonoBehaviour {
	public async void StartSession() {
		ISceneIdentifier sceneIdentifier = new BuiltInSceneIdentifier ("Sample");

		await GlobalSceneNavigator.Instance.Push (new LoadSceneRequest(sceneIdentifier, null, null, null));
	}
}
