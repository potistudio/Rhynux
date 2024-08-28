
using UnityEngine;
using UnityEngine.UI;


public class CanvasLineGrid : Graphic {
	[SerializeField] private bool m_AlignToCenter;

	[SerializeField] private Vector2Int m_GridSize;
	[SerializeField] private Vector2 m_GridOffset;
	[SerializeField] private Vector2 m_GridSpace;

	[SerializeField] private float m_LineWidth;
	[SerializeField] private Color m_LineColor;

	protected override void OnPopulateMesh (VertexHelper vh) {
		vh.Clear();

		Vector2Int size = m_AlignToCenter ? new Vector2Int ((int)(Screen.width / m_GridSpace.x) + 1, (int)(Screen.height / m_GridSpace.y) + 1) : m_GridSize;
		Vector2 offset = m_AlignToCenter ? m_GridOffset + (new Vector2(Screen.width, Screen.height) - size * m_GridSpace) / 2 : m_GridOffset;

		// //* Draw Vertical Line
		for (int x = 0; x < size.x; x++) {
			UIUtilities.MakeLineMesh (vh, new Vector2(x * m_GridSpace.x + offset.x, Screen.height), new Vector2(x * m_GridSpace.x + offset.x, 0f), m_LineWidth, m_LineColor);
		}

		// //* Draw Horizontal Line
		for (int y = 0; y < size.y; y++) {
			UIUtilities.MakeLineMesh (vh, new Vector2(0f, y * m_GridSpace.y + offset.y), new Vector2(Screen.width, y * m_GridSpace.y + offset.y), m_LineWidth, m_LineColor);
		}
	}
}
