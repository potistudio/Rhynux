
using UniRx;

public class InputReferee : UnityEngine.MonoBehaviour {
	[UnityEngine.SerializeField] private MusicPlayer m_MusicPlayer;
	[UnityEngine.SerializeField] private JudgementDisplay m_JudgementDisplay;

	[VContainer.Inject]
	private void Inject (NotesReferee _notesReferee, ReactiveReferee _reactiveReferee) {
		IInputInterface inputInterface = new KeyboardInput (new KeyboardActions());

		inputInterface.OnPressed.Subscribe (_ => {
			_reactiveReferee.JudgeHit (0);
		}).AddTo (this);

		_notesReferee.OnHit.Subscribe (x => {
			string popupMessage = "";

			switch (x) {
				case AccuracyLevel.Perfect:
					popupMessage = "Perfect";
					break;
				case AccuracyLevel.Good:
					popupMessage = "Good";
					break;
				case AccuracyLevel.Miss:
					popupMessage = "Miss";
					break;
				case AccuracyLevel.Pass:
					return;
			}

			m_JudgementDisplay.ShowPopup (popupMessage);
		}).AddTo (this);
	}
}
