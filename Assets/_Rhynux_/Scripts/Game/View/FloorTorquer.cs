using System.Linq;
using UnityEngine;

public class FloorTorquer : MonoBehaviour {
	[SerializeField] private Rigidbody m_Rigidbody;
	[SerializeField] private float m_ForcePower;

	private int m_CurrentIndex;

	private MusicPlayer m_MusicPlayer;
	private SessionFactory m_Session;

	[VContainer.Inject]
	private void Init (MusicPlayer _musicPlayer, SessionFactory _session) {
		m_MusicPlayer = _musicPlayer;
		m_Session = _session;
	}

	private void Update() {
		Note[] notes = m_Session.SessionPool.Notes;

		if (m_CurrentIndex >= notes.Length) {
			Debug.Log ("End");
			return;
		}

		if (m_MusicPlayer.CurrentTime >= notes[m_CurrentIndex].Time) {
			AddTorque (notes[m_CurrentIndex].Position - 1.5f);
			m_CurrentIndex++;
		}
	}

	[Alchemy.Inspector.Button]
	private void AddTorque (float _distance = 0f) {
		m_Rigidbody.AddForceAtPosition (Vector3.down * m_ForcePower, Vector3.right * _distance);
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
}
