
using System.Collections.Generic;
using UnityEngine;

public class NotesObjectGenerator : MonoBehaviour {
	[SerializeField] private GameObject m_NotePrefab;

	private List<GameObject> m_GeneratedNotesObjectList = new();
	public List<GameObject> GeneratedNotesObjectList => m_GeneratedNotesObjectList;

	public List<GameObject> GenerateNotes (List<Note> _notes) {
		Transform notesParent = new GameObject ("Notes Parent").transform;
		m_GeneratedNotesObjectList = new();

		foreach (Note note in _notes) {
			Transform noteObj = Instantiate (m_NotePrefab, new Vector3(note.Position, 0, note.Time), Quaternion.identity).transform;
			noteObj.SetParent (notesParent);

			m_GeneratedNotesObjectList.Add (noteObj.gameObject);
		}

		return m_GeneratedNotesObjectList;
	}
}
