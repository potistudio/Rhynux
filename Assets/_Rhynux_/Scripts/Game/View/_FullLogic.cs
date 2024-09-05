using System.Threading.Tasks;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class _FullLogic : MonoBehaviour {
	[SerializeField] private GameObject m_NotePrefab;
	[SerializeField] private float m_ScrollSpeed;
	[SerializeField] private float m_UserOffset;
	[SerializeField] private float m_FloorWidth;
	[SerializeField] private Transform m_NotesContainer;

	private Chart m_Chart;

	private MusicPlayer m_MusicPlayer;

	private System.Collections.Generic.List<(Note, GameObject)> m_NoteObjects = new();

	[VContainer.Inject]
	private void Init (MusicPlayer _musicPlayer, SessionData _session, Chart _chart) {
		m_Chart = _chart;
		m_MusicPlayer = _musicPlayer;

		foreach (Note note in _session.Notes) {
			GameObject no = Instantiate(m_NotePrefab, Vector3.zero, Quaternion.identity);
			m_NoteObjects.Add ((note, no));
			no.transform.SetParent (m_NotesContainer);
		}

		m_MusicPlayer.Clip = m_Chart.Track.SoundClip;
		m_MusicPlayer.Play();
	}

	private void Update() {
		float noteWidth = m_FloorWidth / 4f;
		foreach (var x in m_NoteObjects) {
			x.Item2.transform.localPosition = new Vector3 (
				aaa(x.Item1.Position, 2.5f),
				0f,
				((x.Item1.Time + m_UserOffset) + (-m_MusicPlayer.CurrentTime)) * m_ScrollSpeed
			);
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
}
