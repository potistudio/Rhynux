
using UnityEngine;

public class GameStarter : MonoBehaviour {
	[SerializeField] private MusicPlayer m_MusicPlayer;
	[SerializeField] private Chart m_Chart;
	[SerializeReference] private INotesGenerator m_NotesGenerator;
	[SerializeField] private InputReferee m_InputReferee;

	[VContainer.Inject]
	private void Init (IInputInterface _inputInterface, ReactiveNotesReferee _refree) {
		System.Collections.Generic.List<Note> notesList = m_NotesGenerator.Generate (m_Chart);
		// InputReferee referee = new (notesList);
		m_InputReferee.Init (_inputInterface, _refree);
	}

	private void Start() {
		m_MusicPlayer.Clip = m_Chart.m_Clip;
		m_MusicPlayer.Play();
	}
}
