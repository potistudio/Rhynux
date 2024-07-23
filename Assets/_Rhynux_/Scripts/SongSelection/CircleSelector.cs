
using UnityEngine;
using UnityEngine.UI;

public class CircleSelector : Graphic {
	[SerializeField] private float m_LineWidth;
	[SerializeField] private Color m_LineColor;
	[SerializeField] private int m_Resolution;

	[SerializeField] private ArcProperty[] m_Arcs;

	protected override void OnPopulateMesh (VertexHelper vh) {
		vh.Clear();

		float radius = Screen.height * 1.5f;
		GetComponent<RectTransform>().anchoredPosition = Vector2.left * (radius - 200f * (Screen.width / 1920f));

		UIUtilities.MakeCircleMesh (vh, transform.position + new Vector3(Screen.width, Screen.height) / 2, radius, m_LineWidth * (Screen.width / 1920f), m_LineColor, m_Resolution);

		for (int i = 0; i < m_Arcs.Length; i++) {
			UIUtilities.MakeArcMesh (vh, (Vector2)transform.position + new Vector2(Screen.width, Screen.height) / 2 + m_Arcs[i].Offset, radius + m_Arcs[i].InnerRadius, m_Arcs[i].LineWidth, m_Arcs[i].Color, m_Arcs[i].StartAngle, m_Arcs[i].EndAngle, m_Resolution);
		}
	}
}

[System.Serializable]
public class ArcProperty {
	[SerializeField] private bool m_IsStatic = false;
	[SerializeField] private float m_InnerRadius = 1f;
	[SerializeField] private float m_LineWidth = 1f;
	[SerializeField] private Vector2 m_Offset = new();
	[SerializeField] private Color m_Color = Color.white;
	[SerializeField, Range(0f, 360f)] private float m_StartAngle = 0f;
	[SerializeField, Range(0f, 360f)] private float m_EndAngle = 360f;

	public bool IsStatic => m_IsStatic;
	public float InnerRadius => m_InnerRadius;
	public float LineWidth => m_LineWidth;
	public Vector2 Offset => m_Offset;
	public Color Color => m_Color;
	public float StartAngle => m_StartAngle;
	public float EndAngle => m_EndAngle;

	public ArcProperty (bool _isStatic, float _innerRadius, float _lineWidth, Vector2 _offset, Color _color, float _startAngle, float _endAngle) {
		m_IsStatic = _isStatic;
		m_InnerRadius = _innerRadius;
		m_LineWidth = _lineWidth;
		m_Offset = _offset;
		m_Color = _color;
		m_StartAngle = _startAngle;
		m_EndAngle = _endAngle;
	}
}
