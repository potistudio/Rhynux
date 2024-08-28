
using UnityEngine;

public class GameStarter : MonoBehaviour {
	[SerializeField] private MusicPlayer m_MusicPlayer;
	private Chart m_Chart;

	[VContainer.Inject]
	private void Inject (SessionManager _sessionManager) {
		m_Chart = _sessionManager.CurrentChart;
	}

	private void Start() {
		m_MusicPlayer.Clip = m_Chart.Track.SoundClip;
		m_MusicPlayer.Play();
	}
}
