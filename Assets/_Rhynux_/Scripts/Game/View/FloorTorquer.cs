using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FloorTorquer : MonoBehaviour {
	//TODO
	[SerializeField] private ChartAsset m_Chart;
	[SerializeField] private MusicPlayer m_MusicPlayer;

	[SerializeField] private Rigidbody m_Rigidbody;
	[SerializeField] private float m_ForcePower;

	private Note[] notes;
	private int m_CurrentIndex;

	private void Start() {
		Chart c = m_Chart.Chart;
		notes = c.Notes.ToArray();
	}

	private void Update() {
		var t = notes[m_CurrentIndex].Time;

		if (m_MusicPlayer.CurrentTime >= t * (60f / m_Chart.Chart.BPM)) {
			AddTorque (notes[m_CurrentIndex].Position - 1.5f);
			m_CurrentIndex++;
		}
	}

	private float aaa (float x, float t) {
		float upper = Mathf.Ceil (t);  // 2.5 -> 3.0
		float lower = Mathf.Floor (t); // 2.5 -> 2.0

		if (x < upper) {
			return x - upper;
		} else {
			return x - lower;
		}
	}

	[Alchemy.Inspector.Button]
	private void AddTorque (float _distance = 0f) {
		Debug.Log (_distance);
		m_Rigidbody.AddForceAtPosition (Vector3.down * m_ForcePower, Vector3.right * _distance);
	}

}
