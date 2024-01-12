
using UnityEngine;

public class MusicPlayer : MonoBehaviour {
	[SerializeField] private AudioSource m_AudioSource;

	public AudioClip Clip { get { return m_AudioSource.clip; } set { m_AudioSource.clip = value; }}

	public void Play() {
		m_AudioSource.Play();
	}
}
