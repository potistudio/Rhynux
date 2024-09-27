using UnityEngine;

public sealed class InputVisualizer : MonoBehaviour {
	[SerializeField] private GameObject[] m_Highlight;

	public void Activate (int lane) {
		m_Highlight[lane].SetActive (true);
	}

	public void Deactivate (int lane) {
		m_Highlight[lane].SetActive (false);
	}
}
