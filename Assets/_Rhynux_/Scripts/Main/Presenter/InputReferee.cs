
using UniRx;

public class InputReferee : UnityEngine.MonoBehaviour {
	[UnityEngine.SerializeField] private MusicPlayer m_MusicPlayer;

	[VContainer.Inject]
	private void Inject (ReactiveReferee _reactiveReferee) {
		IInputInterface inputInterface = new KeyboardInput (new KeyboardActions());

		inputInterface.OnPressed.Subscribe (_ => {
			_reactiveReferee.JudgeHit (0);
		}).AddTo (this);
	}
}
