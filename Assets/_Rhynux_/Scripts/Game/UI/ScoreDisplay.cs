using UnityEngine;

public class ScoreDisplay : MonoBehaviour {
	[SerializeField] private string m_Format;
    [SerializeField] private TMPro.TextMeshProUGUI m_Label;

	private int m_CurrentScore;
	private int m_TargetScore;

	private void Update() {
		if (m_CurrentScore == m_TargetScore)
			return;

		// Rounding a Lerp stalls once the gap drops below one point, so snap the tail end.
		if (Mathf.Abs (m_TargetScore - m_CurrentScore) <= 1)
			m_CurrentScore = m_TargetScore;
		else
			m_CurrentScore = Mathf.RoundToInt (Mathf.Lerp(m_CurrentScore, m_TargetScore, 0.1f));

		string formatted = string.Format (m_Format, m_CurrentScore);
		m_Label.text = formatted;
	}

	public void SetScore (int _score) {
		m_TargetScore = _score;
	}
}
