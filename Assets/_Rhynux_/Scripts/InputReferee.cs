
using UnityEngine;
using UniRx;

/* Controller */
// <-- InputInterface
// --> ReactiveNotesReferee
public class InputReferee : MonoBehaviour {
	[SerializeField] private MusicPlayer m_MusicPlayer;
	private IInputInterface m_InputInterface;

	[VContainer.Inject]
	public void Init (IInputInterface _inputInterface, ReactiveNotesReferee _refree) {
		m_InputInterface = _inputInterface;

		m_InputInterface.OnPressed.Subscribe (_ => {
			_refree.FindNearestNote (m_MusicPlayer.CurrentTime, _);
		}).AddTo (this);
	}
}
