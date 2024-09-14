using UnityEngine;

public class ScoreDisplay : MonoBehaviour {
	[SerializeField] private string m_Format;
    [SerializeField] private TMPro.TextMeshProUGUI m_Label;

	//TODO: tmp
	private void Update() {
		float __time = Time.time;
		SetScore (Mathf.RoundToInt(__time));
	}

	public void SetScore (int _score) {
		string formatted = string.Format (m_Format, _score);
		m_Label.text = formatted;
	}
}
