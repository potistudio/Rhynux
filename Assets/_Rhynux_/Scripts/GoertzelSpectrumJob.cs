
using Unity.Collections;
using Unity.Jobs;

public struct GoertzelSpectrumJob : IJob {
	public NativeArray<float> m_WaveformInput;
	public NativeArray<float> m_SpectrumOutput;
	public int m_SampleRate;

	public int m_SamplesOut;
	public float m_OutputMultiplier;
	public int m_FreqMin;
	public int m_FreqMax;
	public int m_AudioDuration;

	public float m_SmoothingTimeConstant;
	public float m_WindowSkew;

	public void Execute() {
		m_SpectrumOutput[0] = m_WaveformInput[512];
	}
}
