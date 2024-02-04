
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;
using UniRx;

public class FPSCounter : MonoBehaviour {
	[SerializeField] private TextMeshProUGUI m_DisplayText;
	[SerializeField] private float m_UpdateInterval;

	private List<float> m_FPSList = new();

	private void Start() {
		Observable.Interval (System.TimeSpan.FromMilliseconds(m_UpdateInterval))
				  .Subscribe ((_) => {
					  float currentFPS = 1f / Time.deltaTime;
					  m_FPSList.Add (currentFPS);

					  float averageFPS = m_FPSList.Average();

					  m_DisplayText.text = System.Math.Floor(currentFPS).ToString() + " | " + System.Math.Floor(averageFPS).ToString();
				  })
				  .AddTo (this);
	}
}
