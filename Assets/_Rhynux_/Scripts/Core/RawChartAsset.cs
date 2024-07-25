using UnityEngine;

[CreateAssetMenu(fileName = "Raw Chart", menuName = "Rhynux/Raw Chart")]
public class RawChartAsset : ChartAsset {
	[SerializeField] private string m_Title;
	[SerializeField] private string m_Artist;
	[SerializeField] private float m_BPM;
	[SerializeField] private float m_Offset;
	[SerializeField] private AudioClip m_Clip;
	[SerializeField] private SoundTrack m_SoundTrack;
	[SerializeField] private System.Collections.Generic.List<Note> m_Notes;
	[SerializeField] private bool m_Secured;

	public override Chart Chart {
		get {
			return new Chart (m_Title, m_Artist, m_BPM, m_Offset, m_SoundTrack, m_Notes, m_Secured) { Track = m_SoundTrack };
		}
	}
}
