
using UnityEngine;
using UniRx;

public class NotesObjectGenerator : MonoBehaviour {
	[SerializeField] private GameObject m_NotePrefab;

	[VContainer.Inject]
	public void Init (INotesGenerator _notesGenerator) {
		_notesGenerator.OnNotesGenerated.Subscribe (notes => {
			foreach (Note note in notes)
				Instantiate (m_NotePrefab, new Vector3(note.Position, 0, note.Time), Quaternion.identity);
		});
	}
}
