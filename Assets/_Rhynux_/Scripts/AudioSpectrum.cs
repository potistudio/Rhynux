
using UnityEngine;

public class AudioSpectrum : MonoBehaviour {
	[SerializeField] private AudioSource m_AudioSource;

	private float[] m_OutputAudioData = new float[1024];
	public float[] OutputAudioData => m_OutputAudioData;

	private void Update() {
		m_AudioSource.GetOutputData (m_OutputAudioData, 0);
	}
}
