
using UnityEngine;
using FancyScrollView;
using TMPro;


public class ScrollCell : FancyCell<Chart> {
	[SerializeField] private float m_Radius;
	[SerializeField] private float m_StartAngle;
	[SerializeField] private float m_EndAngle;

	public float StartAngle => m_StartAngle;
	public float EndAngle => m_EndAngle;

	private TextMeshProUGUI m_SongTitleText;
	private TextMeshProUGUI m_ArtistNameText;

	public override void UpdateContent (Chart itemData) {
		m_SongTitleText = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
		m_ArtistNameText = transform.GetChild(2).GetComponent<TextMeshProUGUI>();

		m_SongTitleText.text = itemData.Title;
		m_ArtistNameText.text = itemData.Artist;
	}

	public override void UpdatePosition (float position) {
		float currentAngleRad = Mathf.LerpAngle (m_StartAngle, m_EndAngle, position) * Mathf.Deg2Rad;
		float currentScale = Mathf.Lerp (1f, 0.5f, System.Math.Abs(position - 0.5f));

		Vector2 pos = new Vector2 (Mathf.Cos(currentAngleRad), Mathf.Sin(currentAngleRad)) * m_Radius;

		transform.localPosition = pos;
		transform.localScale = Vector3.one * currentScale;

		m_SongTitleText.alpha = Mathf.Lerp (1f, 0f, System.Math.Abs(position - 0.5f));
		m_ArtistNameText.alpha = Mathf.Lerp (1f, 0f, System.Math.Abs(position - 0.5f));
	}
}
