using UnityEngine;
using LitMotion;
using LitMotion.Extensions;

public class TrackArtworkView : MonoBehaviour {
	[SerializeField] private RectTransform m_ImageRect;
	[SerializeField] private UnityEngine.UI.Image m_ImageComponent;

	[SerializeField] private float m_Duration;
	[SerializeField] private Ease m_Ease;

    public void ChangeCoverImage (Sprite _sprite) {
		if (_sprite == null)
			return;

		m_ImageComponent.sprite = _sprite;

		LMotion.Create(Vector3.forward * 8f, Vector3.forward * 2f, m_Duration)
			.WithEase(m_Ease)
			.BindToLocalEulerAngles (m_ImageRect);

		LMotion.Create(Vector3.one * 0.9f, Vector3.one, m_Duration)
			.WithEase(m_Ease)
			.BindToLocalScale(m_ImageRect);

		LMotion.Create (0f, 1f, m_Duration)
			.WithEase (m_Ease)
			.BindToColorA (m_ImageComponent);
	}
}
