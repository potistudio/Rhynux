
using System.Collections.Generic;
using UnityEngine;


public class CanvasDotGrid : MonoBehaviour {
	[SerializeField] private bool m_AlignToCenter;

	[SerializeField] private Vector2Int m_GridSize;
	[SerializeField] private Vector2 m_GridOffset;
	[SerializeField] private Vector2 m_GridSpace;

	[SerializeField] private float m_DotSize;

	[SerializeField] private Sprite m_DotSprite;
	[SerializeField] private Color m_DotColor;

	[SerializeField] private bool m_Noise;
	[SerializeField] private float m_NoiseScale;
	[SerializeField] private int m_NoiseOctaves;
	[SerializeField] private float m_NoisePersistance;
	[SerializeField] private float m_NoiseLacunality;

	private Vector2Int m_ActualSize;
	private float[,] m_NoiseMap;

	private List<UnityEngine.UI.Image> m_Images = new();

	private void Start() {
		m_ActualSize = m_AlignToCenter ? new Vector2Int ((int)(Screen.width / m_GridSpace.x) + 1, (int)(Screen.height / m_GridSpace.y) + 1) : m_GridSize;
		Vector2 offset = m_AlignToCenter ? m_GridOffset + (new Vector2(Screen.width, Screen.height) - m_ActualSize * m_GridSpace) / 2 : m_GridOffset;

		//* Draw Dots
		for (int x = 0; x < m_ActualSize.x; x++) {
			for (int y = 0; y < m_ActualSize.y; y++) {
				DrawDot (new Vector2(x * m_GridSpace.x + offset.x, y * m_GridSpace.y + offset.y), m_DotSize, m_DotColor);
			}
		}

		m_NoiseMap = GenerateNoiseMap (m_ActualSize.x, m_ActualSize.y, m_NoiseScale, m_NoiseOctaves, m_NoisePersistance, m_NoiseLacunality);
	}

	// Realtime Editing
	private void OnValidate() {
		if (!Application.isPlaying)
			return;

		m_NoiseMap = GenerateNoiseMap (m_ActualSize.x, m_ActualSize.y, m_NoiseScale, m_NoiseOctaves, m_NoisePersistance, m_NoiseLacunality);
	}

	private void Update() {
		if (m_Noise) {
			for (int y = 0; y < m_ActualSize.y; y++) {
				for (int x = 0; x < m_ActualSize.x; x++) {
					Color color = m_Images[y * m_ActualSize.x + x].color;
					color.a = m_NoiseMap[x, y];
					m_Images[y * m_ActualSize.x + x].color = color;
				}
			}
		}
	}

	private void DrawDot (Vector2 _position, float _size, Color _color) {
		GameObject imageObject = new GameObject();

		UnityEngine.UI.Image image = imageObject.AddComponent<UnityEngine.UI.Image>();
		RectTransform rect = imageObject.GetComponent<RectTransform>();

		rect.SetParent (transform); // This Transform
		rect.localPosition = _position - (new Vector2(Screen.width, Screen.height) / 2);
		rect.localScale = Vector3.one * _size;

		image.sprite = m_DotSprite;
		image.color = _color;

		m_Images.Add (image);
	}

	private float[,] GenerateNoiseMap (int _width, int _height, float _scale, int _octave, float _persistance, float _lacunarity) {
		float[,] noiseMap = new float[_width, _height];

		//* Seed
		System.Random prng = new System.Random (0);
		Vector2[] octaveOffsets = new Vector2[_octave];
		for (int i = 0; i < _octave; i++) {
			octaveOffsets[i] = new Vector2 (prng.Next (-10000, 10000), prng.Next (-10000, 10000));
		}

		if (_scale <= 0)
			_scale = 0.0001f;

		float maxNoiseHeight = float.MinValue;
		float minNoiseHeight = float.MaxValue;

		for (int y = 0; y < _height; y++) {
			for (int x = 0; x < _width; x++) {
				float amplitude = 1;
				float frequency = 1;
				float noiseHeight = 0;

				for (int i = 0; i < _octave; i++) {
					float sampleX = x / _scale * frequency + octaveOffsets[i].x;
					float sampleY = y / _scale * frequency + octaveOffsets[i].y;

					float perlinValue = Mathf.PerlinNoise (sampleX, sampleY) * 2 - 1;
					noiseHeight += perlinValue * amplitude;

					amplitude *= _persistance;
					frequency *= _lacunarity;
				}

				if (noiseHeight > maxNoiseHeight)
					maxNoiseHeight = noiseHeight;
				else if (noiseHeight < minNoiseHeight)
					minNoiseHeight = noiseHeight;

				noiseMap[x, y] = noiseHeight;
			}
		}

		for (int y = 0; y < _height; y++) {
			for (int x = 0; x < _width; x++) {
				noiseMap[x, y] = Mathf.InverseLerp (minNoiseHeight, maxNoiseHeight, noiseMap[x, y]);
			}
		}

		return noiseMap;
	}
}
