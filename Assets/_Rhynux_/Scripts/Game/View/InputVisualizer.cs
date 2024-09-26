using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class InputVisualizer : MonoBehaviour {
	[SerializeField] private GameObject[] m_Highlight;

	[VContainer.Inject]
	private void Init (IInputHandler _input) {
		_input.OnPressed.Subscribe (_ => {
			Activate (_);
		});

		_input.OnReleased.Subscribe (_ => {
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
