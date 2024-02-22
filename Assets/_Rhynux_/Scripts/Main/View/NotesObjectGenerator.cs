
using UnityEngine;

public class NotesObjectGenerator : MonoBehaviour {
	[SerializeField] private GameObject m_NotePrefab;

	public void GenerateNotes (System.Collections.Generic.List<Note> _notes) {
		Transform notesParent = new GameObject ("Notes Parent").transform;

		foreach (Note note in _notes) {
			Transform noteObj = Instantiate (m_NotePrefab, new Vector3(note.Position, 0, note.Time), Quaternion.identity).transform;
			noteObj.SetParent (notesParent);
		}
	}
}
