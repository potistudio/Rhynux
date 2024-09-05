using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class InputVisualizer : MonoBehaviour {
	[SerializeField] private GameObject m_Highlight;

	private void Start() {
	}

	private void Update() {
		if (Input.GetKeyDown(KeyCode.F))
			m_Highlight.SetActive (true);
		if (Input.GetKeyUp(KeyCode.F))
			m_Highlight.SetActive (false);
	}
}
