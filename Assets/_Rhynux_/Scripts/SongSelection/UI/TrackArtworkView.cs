using UnityEngine;
using MagicTween;

public class TrackArtworkView : MonoBehaviour {
	[SerializeField] private RectTransform m_ImageRect;
	[SerializeField] private UnityEngine.UI.Image m_ImageComponent;

	[SerializeField] private float m_Duration;
	[SerializeField] private Ease m_Ease;

    public void ChangeCoverImage (Sprite _sprite) {
		if (_sprite == null)
			return;

		m_ImageComponent.sprite = _sprite;

		m_ImageRect.TweenLocalEulerAngles (Vector3.forward * 8f, Vector3.forward * 2f, m_Duration)
			.SetEase (m_Ease);

		m_ImageRect.TweenLocalScale (Vector3.one * 0.9f, Vector3.one, m_Duration)
			.SetEase (m_Ease);

		Tween.FromTo (x => {
				Color color = m_ImageComponent.color;
				color.a = x;
				m_ImageComponent.color = color;
			}, 0f, 1f, m_Duration)
			.SetEase (m_Ease);
	}
}
