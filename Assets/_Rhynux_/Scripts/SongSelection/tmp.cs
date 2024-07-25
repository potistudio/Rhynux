using UnityEngine;

public class tmp : MonoBehaviour {
	public AudioSource m_AudioSource;
	public ChartAsset m_Chart;

	void Start() {
		m_AudioSource.clip = m_Chart.Chart.Track.SoundClip;
		m_AudioSource.time = m_Chart.Chart.Track.DropTime;
		m_AudioSource.Play();
	}
}
