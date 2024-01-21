
using UnityEngine;

public class MusicPlayer : MonoBehaviour {
	[SerializeField] private AudioSource m_AudioSource;

	public float CurrentTime => m_AudioSource.time;
	public bool IsPlaying => m_AudioSource.isPlaying;
	public AudioClip Clip { get { return m_AudioSource.clip; } set { m_AudioSource.clip = value; }}

	public void Play() {
		m_AudioSource.Play();
	}

	public void Pause() {
		m_AudioSource.Pause();
	}

	public void Stop() {
		m_AudioSource.Stop();
	}
}
