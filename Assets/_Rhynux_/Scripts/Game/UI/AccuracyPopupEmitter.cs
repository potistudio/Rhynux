using UnityEngine;
using LitMotion;

public class AccuracyPopupEmitter : MonoBehaviour {
	[SerializeField] private GameObject m_PopupObject;

	[SerializeField] private float m_Duration;
	[SerializeField] private float m_Scale;

	private TMPro.TextMeshProUGUI m_PopupText;
	private MotionHandle m_Sequence;

	private void Start() {
		m_PopupText = m_PopupObject.GetComponentInChildren<TMPro.TextMeshProUGUI>();

		// MotionSequenceBuilder sequence = LSequence.Create();
		// m_Sequence.OnComplete (() => m_PopupObject.SetActive(false));

		// m_Sequence.Join (m_PopupObject.transform
		// 	.TweenLocalScale (Vector3.one * m_Scale, Vector3.one, m_Duration)
		// 	.SetEase (Ease.OutCubic)
		// );

		// m_Sequence.Join (m_PopupText
		// 	.TweenAlpha (1f, 0f, m_Duration)
		// 	.SetEase (Ease.OutCubic)
		// );
	}

	public void Emit (AccuracyLevel _accuracyLevel) {
		m_PopupObject.SetActive (true);
		m_PopupText.text = _accuracyLevel.ToString();

		switch (_accuracyLevel) {
			case AccuracyLevel.Perfect:
				m_PopupText.color = new Color (1f, 0.737f, 0.208f);
				break;
			case AccuracyLevel.Good:
				m_PopupText.color = new Color (0.208f, 0.475f, 1f);
				break;
			case AccuracyLevel.Miss:
				m_PopupText.color = new Color (1f, 1f, 1f);
				break;
		}

		m_Sequence.Complete();
	}
}
