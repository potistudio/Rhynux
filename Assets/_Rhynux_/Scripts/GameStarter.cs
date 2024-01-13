
using UnityEngine;

public class GameStarter : MonoBehaviour {
	[SerializeField] private MusicPlayer m_MusicPlayer;
	[SerializeField] private Chart m_Chart;

	private void Start() {
		m_MusicPlayer.Clip = m_Chart.m_Clip;
		m_MusicPlayer.Play();
	}
}
