
using UnityEngine;

public class GameStarter : MonoBehaviour {
	[SerializeField] private MusicPlayer m_MusicPlayer;
	[SerializeField] private AudioClip m_SongClip;

	private void Start() {
		m_MusicPlayer.Clip = m_SongClip;
		m_MusicPlayer.Play();
	}
}
