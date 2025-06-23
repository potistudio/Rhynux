using UnityEngine;
using TMPro;
using LitMotion;
using LitMotion.Extensions;

public sealed class TrackInfoView : MonoBehaviour {
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

	private MotionHandle m_Sequence;

	[VContainer.Inject]
	private void Init() {
		m_TitleTextMesh = m_TitleTextTransform.GetComponent<TextMeshProUGUI>();
		m_ArtistTextMesh = m_ArtistTextTransform.GetComponent<TextMeshProUGUI>();

		m_DefaultTitleTextPosition = m_TitleTextTransform.localPosition;
		m_DefaultArtistTextPosition = m_ArtistTextTransform.localPosition;

		MotionSequenceBuilder sequence = LSequence.Create();

		sequence.Append(
			LMotion.Create(m_DefaultTitleTextPosition.x - m_Backing, m_DefaultTitleTextPosition.x, m_Duration)
				.WithEase(m_Ease)
				.BindToLocalPositionX(m_TitleTextTransform)
		);

		sequence.Join(
			LMotion.Create(m_DefaultArtistTextPosition.x - m_Backing, m_DefaultArtistTextPosition.x, m_Duration)
				.WithEase(m_Ease)
				.WithDelay(m_Delay)
				.BindToLocalPositionX(m_ArtistTextTransform)
		);

		sequence.Join(
			LMotion.Create(0f, 1f, m_Duration)
				.WithEase(m_Ease)
				.BindToColorA(m_TitleTextMesh)
		);

		sequence.Join(
			LMotion.Create(0f, 0.8f, m_Duration)
				.WithDelay(m_Delay)
				.WithEase(m_Ease)
				.BindToColorA(m_ArtistTextMesh)
		);

		m_Sequence = sequence.Run();
		m_Sequence.Preserve();
	}

	public void ChangeInfoContent (string _title, string _artist) {
		if (string.IsNullOrEmpty(_title) || string.IsNullOrEmpty(_artist))
			return;

		m_TitleTextMesh.text = _title;
		m_ArtistTextMesh.text = _artist;

		m_Sequence.Complete();
	}
}
