
using Unity.Collections;

namespace Rhynux.Game {
	[Unity.Burst.BurstCompile]
	public struct GoertzelSpectrumJob : Unity.Jobs.IJob {
		[ReadOnly] public NativeArray<float> m_WaveformInput; // Input
		[WriteOnly] public NativeArray<float> m_SpectrumOutput; // Output

		public int m_SampleRate;
		public int m_SamplesOut;
		public float m_OutputMultiplier;
		public int m_FreqMin;
		public int m_FreqMax;
		public int m_AudioDuration;

		public float m_WindowSkew;

		private readonly float Remap (float _x, float _inMin, float _inMax, float _outMin, float _outMax) {
			return (_x - _inMin) / (_inMax - _inMin) * (_outMax - _outMin) + _outMin;
		}

		private readonly float ApplyWindow (float _posX, bool _truncate, float _skew) {
			float x = _skew > 0 ? (_posX / 2f - 0.5f) / (1f - (_posX / 2f - 0.5f) * 10f * (float)(float)System.Math.Pow((float)_skew, (float)2f)) / (1f / (1f + 10f * (float)(float)System.Math.Pow((float)_skew, (float)2f))) * 2f + 1 : (_posX / 2f + 0.5f) / (1f + (_posX / 2f + 0.5f) * 10f * (float)(float)System.Math.Pow((float)_skew, (float)2f)) / (1f / (1f + 10f * (float)(float)System.Math.Pow((float)_skew, (float)2f))) * 2f - 1f;

			if (_truncate && (float)System.Math.Abs(x) > 1)
				return 0f;

			return 0.54f + 0.46f * (float)(float)System.Math.Cos (x * (float)System.Math.PI);
		}

		private readonly float ApplyWeight (float _freq, float _amount) {
			float f2 = (float)System.Math.Pow (_freq, 2);

			return (float)System.Math.Pow (1.2588966f * 148840000f * (float)System.Math.Pow(f2, 2) / ((f2 + 424.36f) * (float)System.Math.Sqrt((f2 + 11599.29f) * (f2 + 544496.41f)) * (f2 + 148840000f)), _amount);
		}

		private readonly float Ascale (float _x, float _nthRoot, bool _logarithmic, float _dbRange, bool _useAbsoluteValue) {
			return Remap ((float)System.Math.Pow(_x, 1f / _nthRoot), _useAbsoluteValue ? 0f : (float)System.Math.Pow(DBToLinear(-_dbRange), 1f / _nthRoot), 1f, 0f, 1f);
		}

		private readonly float DBToLinear (float _dB) {
			return (float)System.Math.Pow (10f, _dB / 20f);
		}

		private readonly float CalcFreqTilt (float _freq, float _centerFreq = 440f, float _amount = 3f) {
			return (float)System.Math.Abs (_amount) > 0f ? (float)System.Math.Pow (10f, (float)System.Math.Log(_freq / _centerFreq, 2) * _amount / 20f) : 1f;
		}

		private readonly void GenerateFreqBands (ref NativeArray<Freq> _targetArray, int _samples, int _min, int _max) {
			for (int i = 0; i < _samples; i++) {
				_targetArray[i] = new Freq (
					Remap (i - 0.5f, 0f, _samples - 1f, _min, _max),
					Remap (i, 0f, _samples - 1f, _min, _max),
					Remap (i + 0.5f, 0f, _samples - 1f, _min, _max)
				);
			}
		}

		private readonly float CalcGoertzel (NativeArray<float> _waveform, float _coeff) {
			float f1 = 0f, f2 = 0f, sine;

			foreach (float x in _waveform) {
				sine = x + _coeff * f1 - f2;
				f2 = f1;
				f1 = sine;
			}

			return (float)System.Math.Sqrt ((float)System.Math.Pow(f1, 2) + (float)System.Math.Pow(f2, 2) - _coeff * f1 * f2) / _waveform.Length;
		}

		private readonly void CalcGoertzelSpectrum (ref NativeArray<float> _resultArray, NativeArray<float> _waveform) {
			NativeArray<Freq> freqBands = new (m_SamplesOut, Allocator.Temp);
			GenerateFreqBands (ref freqBands, m_SamplesOut, m_FreqMin, m_FreqMax);

			for (int i = 0; i < freqBands.Length; i++) {
				float coeff = 2f * (float)System.Math.Cos (2f * (float)System.Math.PI * freqBands[i].Mid / m_SampleRate);
				_resultArray[i] = CalcGoertzel (_waveform, coeff);
			}

			freqBands.Dispose();
		}

		public void Execute() {
			int FFT_SIZE = (int)System.Math.Round (m_AudioDuration * (m_SampleRate * 0.001f));

			NativeArray<float> audioBuffer = new (FFT_SIZE, Allocator.Temp);
			float normalized = 0f;

			for (int i = 0; i < FFT_SIZE; i++) {
				float x = i * 2f / (FFT_SIZE - 1) - 1;
				float w = ApplyWindow (x, true, m_WindowSkew);

				audioBuffer[i] = m_WaveformInput[i + (AudioSpectrum.SAMPLE_BUFFER_SIZE - FFT_SIZE)] * w;
				normalized += w;
			}

			for (int i = 0; i < audioBuffer.Length; i++) {
				audioBuffer[i] = audioBuffer[i] * (audioBuffer.Length / normalized);
			}

			NativeArray<float> resultBuffer = new (m_SamplesOut, Allocator.Temp);
			CalcGoertzelSpectrum (ref resultBuffer, audioBuffer);

			// The bands only depend on the configured range, so build them once per run
			// rather than rebuilding the whole table for every single bin.
			NativeArray<Freq> freqBands = new (m_SamplesOut, Allocator.Temp);
			GenerateFreqBands (ref freqBands, m_SamplesOut, m_FreqMin, m_FreqMax);

			float gain = DBToLinear (m_OutputMultiplier);

			for (int i = 0; i < resultBuffer.Length; i++) {
				float weighted = resultBuffer[i] * gain * CalcFreqTilt (freqBands[i].Mid, 440f, 0f) * ApplyWeight (freqBands[i].Mid, 0f);
				m_SpectrumOutput[i] = (float)System.Math.Max (Ascale(weighted, 1f, false, 70f, true), 0f);
			}

			audioBuffer.Dispose();
			resultBuffer.Dispose();
			freqBands.Dispose();
		}
	}
}
