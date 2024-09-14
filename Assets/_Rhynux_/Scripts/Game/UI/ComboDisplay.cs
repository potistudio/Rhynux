using UnityEngine;
using MagicTween;

public class ComboDisplay : MonoBehaviour {
	[SerializeField] private string m_Format;
    [SerializeField] private TMPro.TextMeshProUGUI m_Label;

	[Alchemy.Inspector.Title("Animation")]
	[SerializeField] private float m_ScaleMultiplier;
	[SerializeField] private float m_Duration;

	private Tween m_Tween;

	private void Start() {
		m_Tween = m_Label.transform
			.TweenLocalScale (Vector3.one *m_ScaleMultiplier, Vector3.one, m_Duration)
			.SetEase (Ease.OutCubic)
			.SetAutoKill (false)
			.SetAutoPlay (false);
	}

	public void SetValue (int _value) {
		m_Label.gameObject.SetActive (_value > 3);

		string formatted = string.Format (m_Format, _value);
		m_Label.text = formatted;

		m_Tween.Restart();
	}
}
