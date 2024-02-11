
using UnityEngine;

public class GameStarter : MonoBehaviour {
	[SerializeField] private Chart m_Chart;
	[SerializeField] private MusicPlayer m_MusicPlayer;

	private INotesGenerator m_NotesGenerator;

	[VContainer.Inject]
	public void Init (INotesGenerator _notesGenerator) {
		m_NotesGenerator = _notesGenerator;
	}

	private void Start() {
		m_NotesGenerator.Generate (m_Chart);

		m_MusicPlayer.Clip = m_Chart.Clip;
		m_MusicPlayer.Play();
	}
}
