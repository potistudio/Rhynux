
using UnityEngine;
using UniRx;
using System;

public class AudioLooper : MonoBehaviour {
	[SerializeField] private AudioSource m_AudioSource;

	[SerializeField] private AudioClip m_IntroClip;
	[SerializeField] private AudioClip m_LoopClip;

	[SerializeField] private float m_AudioLatencyCorrection;

	private void Start() {
		m_AudioSource.clip = m_IntroClip;
		m_AudioSource.Play();

		Observable.Timer (TimeSpan.FromSeconds(m_IntroClip.length - m_AudioLatencyCorrection * 0.001f))
				  .Subscribe (_ => {
					  m_AudioSource.clip = m_LoopClip;
					  m_AudioSource.Play();

					  m_AudioSource.loop = true;
				  })
				  .AddTo (this);
	}
}
