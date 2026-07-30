
using System.Linq;
using UnityEngine;
using Unity.Jobs;
using Alchemy.Inspector;

#if UNITY_EDITOR
	using UnityEditor;
#endif

public class AudioSpectrum : MonoBehaviour {
	[SerializeField] private AudioSource m_AudioSource;

	[Title("Audio Settings")]
	[SerializeField] private int m_AudioDuration;
	[SerializeField] private int m_MinFrequency;
	[SerializeField] private int m_MaxFrequency;

	[Title("Output Settings")]
	[SerializeField, LabelText("Resolution")] private int m_OutputResolution;
	[SerializeField, LabelText("Multiplier")] private float m_OutputMultiplier;
	[SerializeField, LabelText("Width")] private float m_DrawWidth;

	[Title("Legacy Settings")]
	[SerializeField] private float m_WindowSkew;
	[SerializeField, Range(0f, 1f)] private float m_SmoothingTimeConstant;

	public float[] ProcessedAudioData { get; private set; }

	/// <summary>
	/// Sample window handed to AudioSource.GetOutputData, which requires a power of two.
	/// </summary>
	public const int SAMPLE_BUFFER_SIZE = 8192;

	private readonly float[] m_OutputAudioData = new float[SAMPLE_BUFFER_SIZE];
	private int m_SampleRate = 48000;

	// Kept alive across frames: the job is stateless, so the smoothed spectrum is
	// the only place a previous frame survives. Reusing them also keeps Update()
	// free of per-frame allocations.
	private Unity.Collections.NativeArray<float> m_WaveformBuffer;
	private Unity.Collections.NativeArray<float> m_SpectrumBuffer;
	private float[] m_RawSpectrum;
	private float[] m_SmoothedSpectrum;

	private float Remap (float _x, float _inMin, float _inMax, float _outMin, float _outMax) {
		return (_x - _inMin) / (_inMax - _inMin) * (_outMax - _outMin) + _outMin;
	}

	private void OnValidate() {
		int maxAudioDuration = Mathf.FloorToInt (SAMPLE_BUFFER_SIZE / (m_SampleRate * 0.001f));

		if (m_AudioDuration < 0) m_AudioDuration = 0;
		if (m_AudioDuration > maxAudioDuration) m_AudioDuration = maxAudioDuration;
		if (m_OutputResolution < 0) m_OutputResolution = 0;
		if (m_MinFrequency < 0) m_MinFrequency = 0;
		if (m_MaxFrequency < 0) m_MaxFrequency = 0;
		if (m_DrawWidth < 0f) m_DrawWidth = 0f;
		if (m_WindowSkew < 0f) m_WindowSkew = 0f;
		if (m_SmoothingTimeConstant < 0f) m_SmoothingTimeConstant = 0f;
		if (m_SmoothingTimeConstant > 1f) m_SmoothingTimeConstant = 1f;
	}

	private void Start() {
		m_SampleRate = AudioSettings.outputSampleRate;
	}

	private void Update() {
		if (m_OutputResolution <= 0)
			return;

		// Get Output Waveform
		m_AudioSource.GetOutputData (m_OutputAudioData, 0);

		//* Use Mono *// 17ms
		// ProcessedAudioData = m_GoertzelSpectrumMono.Execute (m_OutputAudioData);

		//* Use Job System *// 5ms
		EnsureBuffers();
		m_WaveformBuffer.CopyFrom (m_OutputAudioData);

		// Create Job
		GoertzelSpectrumJob job = new() {
			m_WaveformInput = m_WaveformBuffer,
			m_SpectrumOutput = m_SpectrumBuffer,
			m_SampleRate = m_SampleRate,
			m_SamplesOut = m_OutputResolution,
			m_OutputMultiplier = m_OutputMultiplier,
			m_FreqMin = m_MinFrequency,
			m_FreqMax = m_MaxFrequency,
			m_AudioDuration = m_AudioDuration,
			m_WindowSkew = m_WindowSkew
		};

		// Execute Job
		JobHandle jobHandle = job.Schedule();
		jobHandle.Complete();

		// Copy Processed Job Buffer to Managed Array
		m_SpectrumBuffer.CopyTo (m_RawSpectrum);

		// Blend against the previous frame. Running this inside the job always started
		// from a freshly zeroed buffer, which reduced the time constant to a plain gain.
		for (int i = 0; i < m_SmoothedSpectrum.Length; i++) {
			float previous = float.IsNaN (m_SmoothedSpectrum[i]) ? 0f : m_SmoothedSpectrum[i];
			float current = float.IsNaN (m_RawSpectrum[i]) ? 0f : m_RawSpectrum[i];

			m_SmoothedSpectrum[i] = previous * m_SmoothingTimeConstant + current * (1f - m_SmoothingTimeConstant);
		}

		ProcessedAudioData = m_SmoothedSpectrum;
	}

	/// <summary>
	/// Allocate the persistent buffers, reallocating only when the resolution changes.
	/// </summary>
	private void EnsureBuffers() {
		if (!m_WaveformBuffer.IsCreated)
			m_WaveformBuffer = new (SAMPLE_BUFFER_SIZE, Unity.Collections.Allocator.Persistent);

		if (m_SpectrumBuffer.IsCreated && m_SpectrumBuffer.Length == m_OutputResolution)
			return;

		if (m_SpectrumBuffer.IsCreated)
			m_SpectrumBuffer.Dispose();

		m_SpectrumBuffer = new (m_OutputResolution, Unity.Collections.Allocator.Persistent);
		m_RawSpectrum = new float[m_OutputResolution];
		m_SmoothedSpectrum = new float[m_OutputResolution];
	}

	private void OnDestroy() {
		if (m_WaveformBuffer.IsCreated)
			m_WaveformBuffer.Dispose();

		if (m_SpectrumBuffer.IsCreated)
			m_SpectrumBuffer.Dispose();
	}

	#if UNITY_EDITOR
		private void OnDrawGizmosSelected() {
			Handles.color = Color.white;

			if (ProcessedAudioData != null) {
				Handles.DrawAAPolyLine (2f, ProcessedAudioData.Select((y, i) => {
					float remappedPosX = Remap (i / (ProcessedAudioData.Length - 1f), 0f, 1f, 0f, -m_DrawWidth);
					return new Vector3 (remappedPosX, y, 0);
				}).ToArray());
			}
		}
	#endif
}

public struct Freq {
	public Freq (float _low, float _mid, float _high) {
		Low = _low;
		Mid = _mid;
		High = _high;
	}

	public float Low { get; private set; }
	public float Mid { get; private set; }
	public float High { get; private set; }
}
