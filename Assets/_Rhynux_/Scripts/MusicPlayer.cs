
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour {
	[SerializeField] private AudioSource m_AudioSource;
	[SerializeField] private AudioClip m_SongClip;

	private void Start() {
		m_AudioSource.clip = m_SongClip;
		m_AudioSource.Play();
	}
}
