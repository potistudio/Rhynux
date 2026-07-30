using UnityEngine;
using LitMotion;
using LitMotion.Extensions;

public class AccuracyPopupEmitter : MonoBehaviour {
	[SerializeField] private GameObject m_PopupObject;

	[SerializeField] private float m_Duration;
	[SerializeField] private float m_Scale;

	private TMPro.TextMeshProUGUI m_PopupText;
	private MotionHandle m_Sequence;

	private void Start() {
		m_PopupText = m_PopupObject.GetComponentInChildren<TMPro.TextMeshProUGUI>();
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

		// Judgements arrive faster than the popup lasts, so drop the one still
		// playing and build a fresh pop. TryCancel tolerates an already-finished handle.
		m_Sequence.TryCancel();

		MotionSequenceBuilder sequence = LSequence.Create();

		sequence.Append (
			LMotion.Create (Vector3.one * m_Scale, Vector3.one, m_Duration)
				.WithEase (Ease.OutCubic)
				.BindToLocalScale (m_PopupObject.transform)
		);

		sequence.Join (
			LMotion.Create (1f, 0f, m_Duration)
				.WithEase (Ease.OutCubic)
				.BindToColorA (m_PopupText)
		);

		m_Sequence = sequence.Run();
	}
}
