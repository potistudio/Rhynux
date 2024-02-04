
using UnityEngine;

#if UNITY_EDITOR
	using System.Linq;
	using UnityEditor;
#endif

public class AudioSpectrum : MonoBehaviour {
	[SerializeField] private AudioSource m_AudioSource;

	private float[] m_OutputAudioData = new float[1024];
	public float[] OutputAudioData => m_OutputAudioData;

	private void Update() {
		m_AudioSource.GetOutputData (m_OutputAudioData, 0);
	}

	#if UNITY_EDITOR
		private void OnDrawGizmos() {
			Handles.color = Color.white;
			Handles.DrawAAPolyLine (2f, OutputAudioData.Select((x, i) => new Vector3(i, x * 32f, 0)).ToArray());
		}
	#endif
}
