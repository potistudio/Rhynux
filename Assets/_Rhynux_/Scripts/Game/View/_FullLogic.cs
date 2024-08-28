using System.Threading.Tasks;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class _FullLogic : MonoBehaviour {
	[SerializeField] public ChartAsset m_ChartAsset;
	[SerializeField] private GameObject m_NotePrefab;
	[SerializeField] private MusicPlayer m_MusicPlayer;
	[SerializeField] private float m_ScrollSpeed;
	[SerializeField] private float m_UserOffset;
	[SerializeField] private float m_FloorWidth;

	private Chart m_Chart;
	private System.Collections.Generic.List<(Note, GameObject)> m_NoteObjects = new();

	public void Main() {
		m_Chart = m_ChartAsset.Chart;

		foreach (Note note in m_Chart.Notes) {
			m_NoteObjects.Add ((note, Instantiate(m_NotePrefab, Vector3.zero, Quaternion.identity)));
		}

		m_MusicPlayer.Clip = m_Chart.Track.SoundClip;
		m_MusicPlayer.Play();
	}

	private void Update() {
		float noteWidth = m_FloorWidth / 4f;
		foreach (var x in m_NoteObjects) {
			x.Item2.transform.position = new Vector3 (
				x.Item1.Position * 1.5f - m_FloorWidth / 2f - noteWidth / 2f,
				0f,
				((x.Item1.Time + m_Chart.Offset + m_UserOffset) + ((m_Chart.BPM / 60f) * -m_MusicPlayer.CurrentTime)) * m_ScrollSpeed
			);
		}
	}
}
