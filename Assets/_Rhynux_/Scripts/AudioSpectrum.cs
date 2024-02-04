
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

	private float Remap (float _x, float _inMin, float _inMax, float _outMin, float _outMax) {
		return (_x - _inMax) / (_inMax - _inMin) * (_outMax - _outMin) + _outMin;
	}

	private float ApplyWindow (float _posX, bool _truncate, float _skew) {
		float x = _skew > 0 ? (_posX / 2f - 0.5f) / (1f - (_posX / 2f - 0.5f) * 10f * Mathf.Pow(_skew, 2)) / (1f / (1f + 10f * Mathf.Pow(_skew, 2))) * 2f + 1 : (_posX / 2f + 0.5f) / (1f + (_posX / 2f + 0.5f) * 10f * Mathf.Pow(_skew, 2)) / (1f / (1f + 10f * Mathf.Pow(_skew, 2))) * 2f - 1f;

		if (_truncate && Mathf.Abs(x) > 1)
			return 0f;

		return 0.54f + 0.46f * Mathf.Cos (x * Mathf.PI);
	}

	private float ApplyWeight (float _freq, float _amount) {
		float f2 = Mathf.Pow (_freq, 2);

		return Mathf.Pow (1.2588966f * 148840000f * Mathf.Pow(f2, 2) / ((f2 + 424.36f) * Mathf.Sqrt((f2 + 11599.29f) * (f2 + 544496.41f)) * (f2 + 148840000f)), _amount);
	}

	private float Ascale (float _x, float _nthRoot, bool _logarithmic, float _dbRange, bool _useAbsoluteValue) {
		return Remap (Mathf.Pow(_x, 1f / _nthRoot), _useAbsoluteValue ? 0f : Mathf.Pow(DBToLinear(-_dbRange), 1f / _nthRoot), 1f, 0f, 1f);
	}

	private float DBToLinear (float _dB) {
		return Mathf.Pow (10f, _dB / 20f);
	}

	private float CalcFreqTilt (float _freq, float _centerFreq = 440f, float _amount = 3f) {
		return Mathf.Abs (_amount) > 0f ? Mathf.Pow (10f, Mathf.Log(_freq / _centerFreq, 2) * _amount / 20f) : 1f;
	}

	private Freq[] GenerateFreqBands (int _samples, int _min, int _max) {
		Freq[] freqArray = new Freq[_samples];

		for (int i = 0; i < _samples; i++) {
			freqArray[i] = new Freq (
				Remap (i - 0.5f, 0f, _samples - 1f, _min, _max),
				Remap (i, 0f, _samples - 1f, _min, _max),
				Remap (i + 0.5f, 0f, _samples - 1f, _min, _max)
			);
		}

		return freqArray;
	}

	private float CalcGoertzel (float[] _waveform, float _coeff) {
		float f1 = 0f, f2 = 0f, sine;

		foreach (float x in _waveform) {
			sine = x + _coeff * f1 - f2;
			f2 = f1;
			f1 = sine;
		}

		return Mathf.Sqrt (Mathf.Pow(f1, 2) + Mathf.Pow(f2, 2) - _coeff * f1 * f2) / _waveform.Length;
	}

	private float[] CalcGoertzelSpectrum (float[] _waveform) {
		return GenerateFreqBands (SAMPLES_OUT, MIN_FREQ, MAX_FREQ).Select (x => {
			float coeff = 2f * Mathf.Cos (2f * Mathf.PI * x.Mid / m_SampleRate);
			return CalcGoertzel (_waveform, coeff);
		}).ToArray();
	}

	private void ApplySmoothingTimeConstant (ref float[] _targetArray, in float[] _sourceArray, float _factor = 0.5f) {
		for (int i = 0; i < _targetArray.Length; i++) {
			_targetArray[i] = (float.NaN == _targetArray[i] ? 0f : _targetArray[i]) * _factor + (float.NaN == _sourceArray[i] ? 0f : _sourceArray[i]) * (1f - _factor);
		}
	}

	private void Start() {
		m_SampleRate = AudioSettings.outputSampleRate;
	}

	private void Update() {
		// Get Output Waveform
		m_AudioSource.GetOutputData (m_OutputAudioData, 0);

		int FFT_SIZE = Mathf.RoundToInt (AUDIO_DURATION * (m_SampleRate * 0.001f));

		float[] audioBuffer = new float[FFT_SIZE];
		float normalized = 0f;

		for (int i = 0; i < FFT_SIZE; i++) {
			float x = i * 2f / (FFT_SIZE - 1) - 1;
			float w = ApplyWindow (x, true, WINDOW_SKEW);

			audioBuffer[i] = m_OutputAudioData[i + (8196 - FFT_SIZE)] * w;
			normalized += w;
		}

		audioBuffer = audioBuffer.Select (x => x * (audioBuffer.Length / normalized)).ToArray();

		float[] resultBuffer = CalcGoertzelSpectrum (audioBuffer);

		float[] dataArray = new float[resultBuffer.Length];
		ApplySmoothingTimeConstant (ref dataArray, resultBuffer.Select((x, i) => {
			Freq[] freqBands = GenerateFreqBands (SAMPLES_OUT, MIN_FREQ, MAX_FREQ);
			return x * DBToLinear (OUTPUT_MULTIPLIER) * CalcFreqTilt (freqBands[i].Mid, 440f, 0f) * ApplyWeight (freqBands[i].Mid, 0f);
		}).ToArray(), SMOOTHING_TIME_CONSTANT);
		float[] processedResult = dataArray.Select((x, i) => Mathf.Max(Ascale(x, 1f, false, 70f, true), 0f)).ToArray();
		ProcessedAudioData = processedResult;
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
