
using UnityEngine;

public class NotesObjectGenerator : MonoBehaviour {
	[SerializeField] private GameObject m_NotePrefab;

	public void Generate (System.Collections.Generic.List<Note> _notes) {
		foreach (Note note in _notes)
			Instantiate (m_NotePrefab, new Vector3(note.Position, 0, note.Time), Quaternion.identity);
	}
}
