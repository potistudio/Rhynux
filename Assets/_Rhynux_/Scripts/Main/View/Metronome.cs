
using UnityEngine;
using Alchemy.Inspector;

public class Metronome : MonoBehaviour {
	[SerializeField] private float m_BPM;
	[SerializeField] private AudioClip m_ClickSound;
	[SerializeField] private AudioSource m_ClickAudioSource;
	[SerializeField] private AudioSource m_ReferenceAudioSource;

	public int BeatCount => m_BeatCount;
	public float CurrentTime => m_ReferenceAudioSource.time - m_LastBeatTime;

	[SerializeField, ReadOnly, HideInEditMode] private int m_BeatCount = 0;
	private float m_LastBeatTime = 0f;

	private void Update() {
		float currentTime = m_ReferenceAudioSource.time;
		float beatIntervalTime = 60f / m_BPM;

		if (currentTime >= m_LastBeatTime + beatIntervalTime * m_BeatCount) {
			m_ClickAudioSource.PlayOneShot (m_ClickSound);
			m_BeatCount++;
		}
	}

	[Button, HideInEditMode]
	public void ResetBeatCount() {
		m_BeatCount = 0;
		m_LastBeatTime = m_ReferenceAudioSource.time;
	}
}
