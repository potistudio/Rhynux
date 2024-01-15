using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class NotesReferee : MonoBehaviour {
	[SerializeField] private AudioSource m_AudioSource;
	[SerializeField] private Chart m_NotesChart;
	[SerializeReference] private INotesGenerator m_NotesGenerator;
	private IInputInterface m_InputInterface;

	private List<Note> m_NotesList = new();
	private int m_NoteIndex = 0;

	[VContainer.Inject]
	public void Init (IInputInterface _inputInterface) {
		m_InputInterface = _inputInterface;
		m_InputInterface.OnPressed.Subscribe (_ => { Debug.Log (_); });
	}

	private void Start() {
		m_NotesList = m_NotesGenerator.Generate (m_NotesChart);
	}

	private void Update() {
		if (m_AudioSource.time >= m_NotesList[m_NoteIndex].Time) {
			Debug.Log (m_NotesList[m_NoteIndex].Pos);
			m_NoteIndex++;
		}
	}
}
