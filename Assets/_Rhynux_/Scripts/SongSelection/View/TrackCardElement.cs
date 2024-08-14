
using UnityEngine;
using FancyScrollView;
using TMPro;


public class TrackCardElement : FancyCell<Chart> {
	[SerializeField] private float m_Radius;
	[SerializeField] private float m_StartAngle;
	[SerializeField] private float m_EndAngle;

	public float StartAngle => m_StartAngle;
	public float EndAngle => m_EndAngle;

	[SerializeField] private TextMeshProUGUI m_TitleTextField;
	[SerializeField] private TextMeshProUGUI m_ArtistTextField;

	public override void UpdateContent (Chart itemData) {
		m_TitleTextField.text = itemData.Title;
		m_ArtistTextField.text = itemData.Artist;
	}

	public override void UpdatePosition (float position) {
		float currentAngleRad = Mathf.LerpAngle (m_StartAngle, m_EndAngle, position) * Mathf.Deg2Rad;
		float currentScale = Mathf.Lerp (1f, 0.5f, System.Math.Abs(position - 0.5f));

		Vector2 pos = new Vector2 (Mathf.Cos(currentAngleRad), Mathf.Sin(currentAngleRad)) * m_Radius;

		transform.localPosition = pos;
		transform.localScale = Vector3.one * currentScale;

		m_TitleTextField.alpha = Mathf.Lerp (1f, 0f, System.Math.Abs(position - 0.5f));
		m_ArtistTextField.alpha = Mathf.Lerp (1f, 0f, System.Math.Abs(position - 0.5f));
	}
}
