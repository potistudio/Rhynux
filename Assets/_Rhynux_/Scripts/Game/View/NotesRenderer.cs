using System.Collections.Generic;
using UnityEngine;

public sealed class NotesRenderer : MonoBehaviour {
	[SerializeField] private GameObject m_NotePrefab;
	[SerializeField] private float m_ScrollSpeed;
	[SerializeField] private float m_UserOffset;
	[SerializeField] private float m_FloorWidth;

	private Chart m_Chart;
	private List<(Note note, GameObject obj)> m_NoteObjects = new();

	[VContainer.Inject]
	public void Inject(Chart _chart) {
		m_Chart = _chart;

		foreach (Note note in m_Chart.Notes) {
			GameObject noteObject = Instantiate(m_NotePrefab);
			m_NoteObjects.Add((note, noteObject));

			noteObject.transform.position = new Vector3(note.Position, 0, note.Time);
			noteObject.transform.SetParent(transform);
		}
	}

	private void Update() {
		//TODO: Use TimeManager to get the current time
		const float CURRENT_TIME = 0f;

		float noteWidth = m_FloorWidth / 4f;
		foreach (var x in m_NoteObjects) {
			x.obj.transform.localPosition = new Vector3(
				(x.note.Position - 1.5f) * 1.5f,
				0f,
				((x.note.Time + m_UserOffset) + (-CURRENT_TIME)) * m_ScrollSpeed * 0.001f
			);
		}
	}
}
