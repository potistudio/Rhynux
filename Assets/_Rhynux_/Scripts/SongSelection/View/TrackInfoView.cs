using UnityEngine;
using TMPro;
using MagicTween;

public class TrackInfoView : MonoBehaviour {
	[SerializeField] private Transform m_TitleTextTransform;
	[SerializeField] private Transform m_ArtistTextTransform;

	private TextMeshProUGUI m_TitleTextMesh;
	private TextMeshProUGUI m_ArtistTextMesh;

	private Vector3 m_DefaultTitleTextPosition;
	private Vector3 m_DefaultArtistTextPosition;

	[SerializeField] private float m_Backing;
	[SerializeField] private float m_Duration;
	[SerializeField] private float m_Delay;

	[SerializeField] private Ease m_Ease;

	private Sequence m_Sequence;

	[VContainer.Inject]
	private void Init() {
		m_TitleTextMesh = m_TitleTextTransform.GetComponent<TextMeshProUGUI>();
		m_ArtistTextMesh = m_ArtistTextTransform.GetComponent<TextMeshProUGUI>();

		m_DefaultTitleTextPosition = m_TitleTextTransform.localPosition;
		m_DefaultArtistTextPosition = m_ArtistTextTransform.localPosition;

		m_Sequence = Sequence.Create();
		m_Sequence.SetAutoKill (false);
		m_Sequence.SetAutoPlay (false);

		m_Sequence.Append (
			m_TitleTextTransform.TweenLocalPositionX (m_DefaultTitleTextPosition.x - m_Backing, m_DefaultTitleTextPosition.x, m_Duration)
				.SetEase (m_Ease)
		);

		m_Sequence.Join (
			m_ArtistTextTransform.TweenLocalPositionX (m_DefaultArtistTextPosition.x - m_Backing, m_DefaultArtistTextPosition.x, m_Duration)
				.SetDelay (m_Delay)
				.SetEase (m_Ease)
		);

		m_Sequence.Join (
			Tween.FromTo (x => m_TitleTextMesh.alpha = x, 0f, 1f, m_Duration)
				.SetEase (m_Ease)
		);

		m_Sequence.Join (
			Tween.FromTo (x => m_ArtistTextMesh.alpha = x, 0f, 1f, m_Duration)
				.SetDelay (m_Delay)
				.SetEase (m_Ease)
		);
	}

	public void ChangeInfoContent (string _title, string _artist) {
		if (string.IsNullOrEmpty(_title) || string.IsNullOrEmpty(_artist))
			return;

		m_TitleTextMesh.text = _title;
		m_ArtistTextMesh.text = _artist;

		m_Sequence.Restart();
	}
}
