
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using MagicTween;
using VContainer;

public class SongInfoView : MonoBehaviour {
	[SerializeField] private RectTransform m_SongInfoContainer;
	[SerializeField] private RectTransform m_SongCoverRect;
	[SerializeField] private Image m_SongCoverContainer;

	[SerializeField] private float m_Backing;
	[SerializeField] private float m_Duration;
	[SerializeField] private Ease m_Ease;

	private TextMeshProUGUI m_SongTitle;
	private TextMeshProUGUI m_SongDescription;

	private Vector3 m_DefaultContainerPosition;

	[Inject]
	private void Init() {
		m_SongTitle = m_SongInfoContainer.GetChild(0).GetComponent<TextMeshProUGUI>();
		m_SongDescription = m_SongInfoContainer.GetChild(1).GetComponent<TextMeshProUGUI>();

		m_DefaultContainerPosition = m_SongInfoContainer.localPosition;
	}


	public void ChangeInfoContent (string _title, string _artist) {
		if (string.IsNullOrEmpty(_title) || string.IsNullOrEmpty(_artist))
			return;

		m_SongTitle.text = _title;
		m_SongDescription.text = _artist;

		//* Animation
		m_SongInfoContainer.TweenLocalPositionX (m_DefaultContainerPosition.x - m_Backing, m_DefaultContainerPosition.x, m_Duration)
			.SetEase (m_Ease);

		Tween.FromTo (x => m_SongTitle.alpha = x, 0f, 1f, m_Duration)
			.SetEase (m_Ease);
	}

	public void ChangeCoverImage (Sprite _sprite) {
		if (_sprite == null)
			return;

		m_SongCoverContainer.sprite = _sprite;

		m_SongCoverRect.TweenLocalEulerAngles (Vector3.forward * 8f, Vector3.forward * 2f, m_Duration)
			.SetEase (m_Ease);

		m_SongCoverRect.TweenLocalScale (Vector3.one * 0.9f, Vector3.one, m_Duration)
			.SetEase (m_Ease);

		Tween.FromTo (x => {
				Color color = m_SongCoverContainer.color;
				color.a = x;
				m_SongCoverContainer.color = color;
			}, 0f, 1f, m_Duration)
			.SetEase (m_Ease);
	}
}
