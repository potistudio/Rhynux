using UnityEngine;

public sealed class TrackCardElement : FancyScrollView.FancyCell<Chart> {
	[SerializeField] private TMPro.TextMeshProUGUI m_TitleTextField;
	[SerializeField] private TMPro.TextMeshProUGUI m_ArtistTextField;

	[SerializeField] private float m_Cosine = 0.259f;

	private const int CANVAS_HEIGHT = 1080;

	public override void UpdateContent (Chart itemData) {
		m_TitleTextField.text = itemData.Title;
		m_ArtistTextField.text = itemData.Artist;
	}

	public override void UpdatePosition (float position) {
		float y = position * CANVAS_HEIGHT;
		float x = y * m_Cosine;

		transform.localPosition = new Vector3 (x, y, 0f);
	}
}
