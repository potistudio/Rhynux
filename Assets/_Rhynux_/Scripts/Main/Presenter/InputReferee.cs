
using UnityEngine;
using UniRx;

public class InputReferee : MonoBehaviour {
	[SerializeField] private MusicPlayer m_MusicPlayer;

	private ReactiveReferee m_NotesReferee = default;

	[VContainer.Inject]
	public void Init (IInputInterface _inputInterface, INotesGenerator _notesGenerator) {
		_notesGenerator.OnNotesGenerated.Subscribe (notes => {
			m_NotesReferee = new ReactiveReferee (notes);
		}).AddTo (this);

		_inputInterface.OnPressed.Subscribe (_ => {
		}).AddTo (this);
	}
}
