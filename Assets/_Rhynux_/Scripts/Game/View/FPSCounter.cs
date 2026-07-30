
using UnityEngine;
using UniRx;

public class FPSCounter : MonoBehaviour {
	[SerializeField] private TMPro.TextMeshProUGUI m_DisplayText;
	[SerializeField] private float m_UpdateInterval;

	// Running mean. Keeping every sample in a List grew without bound over a session
	// and re-averaged the whole history on every tick.
	private double m_FPSSum;
	private long m_SampleCount;

	private void Start() {
		Observable.Interval (System.TimeSpan.FromMilliseconds(m_UpdateInterval))
				  .Subscribe (_ => {
					  float currentFPS = 1f / Time.deltaTime;

					  m_FPSSum += currentFPS;
					  m_SampleCount++;

					  double averageFPS = m_FPSSum / m_SampleCount;

					  m_DisplayText.text = System.Math.Floor (currentFPS).ToString() + " | " + System.Math.Floor (averageFPS).ToString();
				  })
				  .AddTo (this);
	}
}
