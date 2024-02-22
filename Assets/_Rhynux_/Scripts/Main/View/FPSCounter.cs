
using System.Linq;
using UnityEngine;
using UniRx;

public class FPSCounter : MonoBehaviour {
	[SerializeField] private TMPro.TextMeshProUGUI m_DisplayText;
	[SerializeField] private float m_UpdateInterval;

	private readonly System.Collections.Generic.List<float> m_FPSHistory = new();

	private void Start() {
		Observable.Interval (System.TimeSpan.FromMilliseconds(m_UpdateInterval))
				  .Subscribe (_ => {
					  float currentFPS = 1f / Time.deltaTime;
					  m_FPSHistory.Add (currentFPS);

					  float averageFPS = m_FPSHistory.Average();

					  m_DisplayText.text = System.Math.Floor(currentFPS).ToString() + " | " + System.Math.Floor(averageFPS).ToString();
				  })
				  .AddTo (this);
	}
}
