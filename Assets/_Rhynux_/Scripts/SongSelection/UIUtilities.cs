
using UnityEngine;

public static class UIUtilities {
	/// <summary>
	/// Make Line Mesh and Add to VertexHelper
	/// </summary>
	/// <param name="_vh">VertexHelper Called from Unity Graphic</param>
	/// <param name="_from">Line from</param>
	/// <param name="_to">Line to</param>
	/// <param name="_lineWidth">Line Width</param>
	/// <param name="_color">Line Color</param>
	public static void MakeLineMesh (UnityEngine.UI.VertexHelper _vh, Vector2 _from, Vector2 _to, float _lineWidth = 10f, Color _color = default) {
		Vector2 screenSize = new Vector2 (Screen.width, Screen.height);

		//= 座標の準備
		Vector2 v1 = _from + (new Vector2((_from - _to).y, -(_from - _to).x).normalized * _lineWidth) - (screenSize / 2); // From Up
		Vector2 v2 = _from + (new Vector2(-(_from - _to).y, (_from - _to).x).normalized * _lineWidth) - (screenSize / 2); // From Down

		Vector2 v3 = _to + (new Vector2((_to - _from).y, -(_to - _from).x).normalized * _lineWidth) - (screenSize / 2); // To Up
		Vector2 v4 = _to + (new Vector2(-(_to - _from).y, (_to - _from).x).normalized * _lineWidth) - (screenSize / 2); // To Down

		//= 頂点の追加
		UIVertex vert1 = UIVertex.simpleVert;
		vert1.position = v1;
		vert1.color = _color;
		UIVertex vert2 = UIVertex.simpleVert;
		vert2.position = v2;
		vert2.color = _color;
		UIVertex vert3 = UIVertex.simpleVert;
		vert3.position = v3;
		vert3.color = _color;
		UIVertex vert4 = UIVertex.simpleVert;
		vert4.position = v4;
		vert4.color = _color;

		//= 矩形の描画
		_vh.AddUIVertexQuad (new UIVertex[]{vert1, vert2, vert3, vert4});
	}

	public static void MakeCircleMesh (UnityEngine.UI.VertexHelper _vh, Vector2 _position, float _radius, float _width, Color _color, int _resolution = 64) {
		float deltaAngle = Mathf.PI * 2 / _resolution;

		for (int i = 0; i < _resolution; i++) {
			float angle = deltaAngle * i;

			Vector2 from = _position + new Vector2 (Mathf.Cos(angle), Mathf.Sin(angle)) * _radius;
			Vector2 to = _position + new Vector2 (Mathf.Cos(angle + deltaAngle), Mathf.Sin(angle + deltaAngle)) * _radius;

			MakeLineMesh (_vh, from, to, _width, _color);
		}
	}

	public static void MakeArcMesh (UnityEngine.UI.VertexHelper _vh, Vector2 _position, float _radius, float _width, Color _color, float start, float end, int _resolution = 64) {
		float deltaAngle = Mathf.PI * 2 / _resolution;

		for (int i = 0; i < _resolution; i++) {
			float angle = deltaAngle * i;

			if (angle > end * Mathf.Deg2Rad)
				break;

			angle += start * Mathf.Deg2Rad;

			Vector2 from = _position + new Vector2 (Mathf.Cos(angle), Mathf.Sin(angle)) * _radius;
			Vector2 to = _position + new Vector2 (Mathf.Cos(angle + deltaAngle), Mathf.Sin(angle + deltaAngle)) * _radius;

			MakeLineMesh (_vh, from, to, _width, _color);
		}
	}
}
