
using UnityEngine;
using System.Linq;

#if UNITY_EDITOR
	using UnityEditor;
#endif

public class AudioSpectrum : MonoBehaviour {
	[SerializeField] private AudioSource m_AudioSource;

	private float[] m_OutputAudioData = new float[8196];
	public float[] OutputAudioData => m_OutputAudioData;
	public float[] ProcessedAudioData { get; private set; }

	private int m_SampleRate = 48000;

	private const int AUDIO_DURATION = 160;
	private const int SAMPLES_OUT = 200;
	private const int MIN_FREQ = 20;
	private const int MAX_FREQ = 400;
	private const float OUTPUT_MULTIPLIER = 48f;

	private const float WINDOW_SKEW = 0f;
	private const float SMOOTHING_TIME_CONSTANT = 0f;

	private GoertzelSpectrumMono m_GoertzelSpectrumMono;

	private void Start() {
		m_SampleRate = AudioSettings.outputSampleRate;
		m_GoertzelSpectrumMono = new (SAMPLES_OUT, m_SampleRate, MIN_FREQ, MAX_FREQ, SMOOTHING_TIME_CONSTANT, WINDOW_SKEW, OUTPUT_MULTIPLIER, AUDIO_DURATION);
	}

	private void Update() {
		// Get Output Waveform
		m_AudioSource.GetOutputData (m_OutputAudioData, 0);

		//* Use Mono *// 17ms
		ProcessedAudioData = m_GoertzelSpectrumMono.Execute(m_OutputAudioData);

		//* Use Job System *// ?ms
		// Prepare Output Buffer
		float[] processedSpectrum = new float[SAMPLES_OUT];
		Unity.Collections.NativeArray<float> processedSpectrumBuffer = new (SAMPLES_OUT, Unity.Collections.Allocator.TempJob);

		// Prepare Waveform Data as NativeArray
		Unity.Collections.NativeArray<float> source = new (8196, Unity.Collections.Allocator.TempJob);
		source.CopyFrom (m_OutputAudioData);

		// Create Job
		GoertzelSpectrumJob job = new() {
			m_WaveformInput = source,
			m_SpectrumOutput = processedSpectrumBuffer,
			m_SampleRate = m_SampleRate,
			m_SamplesOut = SAMPLES_OUT,
			m_OutputMultiplier = OUTPUT_MULTIPLIER,
			m_FreqMin = MIN_FREQ,
			m_FreqMax = MAX_FREQ,
			m_AudioDuration = AUDIO_DURATION,
			m_SmoothingTimeConstant = SMOOTHING_TIME_CONSTANT,
			m_WindowSkew = WINDOW_SKEW
		};

		// Execute Job
		JobHandle jobHandle = job.Schedule();
		jobHandle.Complete();

		// Copy Processed Job Buffer to Managed Array
		processedSpectrumBuffer.CopyTo (processedSpectrum);
		Debug.Log (processedSpectrum[0]);

		// Dispose NativeArray
		source.Dispose();
		processedSpectrumBuffer.Dispose();
	}

	#if UNITY_EDITOR
		private void OnDrawGizmos() {
			Handles.color = Color.white;

			if (ProcessedAudioData != null)
				Handles.DrawAAPolyLine (2f, ProcessedAudioData.Select((x, i) => new Vector3(i * 2f, x, 0)).ToArray());
		}
	#endif
}

public struct Freq {
	public Freq (float _low, float _mid, float _high) {
		this.Low = _low;
		this.Mid = _mid;
		this.High = _high;
	}

	public float Low { get; private set; }
	public float Mid { get; private set; }
	public float High { get; private set; }
}
