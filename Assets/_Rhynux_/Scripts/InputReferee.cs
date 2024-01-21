
using UnityEngine;
using UniRx;

/* Controller */
// <-- InputInterface
// --> ReactiveNotesReferee
public class InputReferee : MonoBehaviour {
	[SerializeField] private MusicPlayer m_MusicPlayer;

	private ReactiveNotesReferee m_NotesReferee = default;

	[VContainer.Inject]
	public void Init (IInputInterface _inputInterface, INotesGenerator _notesGenerator) {
		_notesGenerator.OnNotesGenerated.Subscribe (notes => {
			m_NotesReferee = new ReactiveNotesReferee (notes);
		}).AddTo (this);

		_inputInterface.OnPressed.Subscribe (_ => {
			int ind = m_NotesReferee.FindNearestNote (m_MusicPlayer.CurrentTime, _);
			Debug.Log (ind);
		}).AddTo (this);
	}
}
