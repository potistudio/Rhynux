using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class InputVisualizer : MonoBehaviour {
	[SerializeField] private GameObject[] m_Highlight;

	[VContainer.Inject]
	private void Init (InputHandlerFactory _factory) {
		IInputHandler inputHandler = _factory.CreateInputHandler (InputMode.Auto);

		inputHandler.OnPressed.Subscribe (_ => {
			Activate (_);
		});

		inputHandler.OnReleased.Subscribe (_ => {
			Deactivate (_);
		});
	}

	private void Activate (int lane) {
		m_Highlight[lane].SetActive (true);
	}

	private void Deactivate (int lane) {
		m_Highlight[lane].SetActive (false);
	}
}
