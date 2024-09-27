using UnityEngine;

public class ScoreDisplay : MonoBehaviour {
	[SerializeField] private string m_Format;
    [SerializeField] private TMPro.TextMeshProUGUI m_Label;

	private int m_CurrentScore;
	private int m_TargetScore;

	private void Update() {
		m_CurrentScore = Mathf.RoundToInt (Mathf.Lerp(m_CurrentScore, m_TargetScore, 0.1f));
	}

	private void FixedUpdate() {
		string formatted = string.Format (m_Format, m_CurrentScore);
		m_Label.text = formatted;
	}

	public void SetScore (int _score) {
		m_TargetScore = _score;
	}
}
