
using UnityEngine;

public class GameStarter : MonoBehaviour {
	[SerializeField] private MusicPlayer m_MusicPlayer;
	[SerializeField] private Chart m_Chart;
	[SerializeReference] private INotesGenerator m_NotesGenerator;
	[SerializeField] private InputReferee m_InputReferee;
	[SerializeField] private NotesObjectGenerator m_NotesObjectGenerator;

	private void Start() {
		System.Collections.Generic.List<Note> notesList = m_NotesGenerator.Generate (m_Chart);
		m_NotesObjectGenerator.Generate (notesList);

		m_MusicPlayer.Clip = m_Chart.m_Clip;
		m_MusicPlayer.Play();
	}
}
