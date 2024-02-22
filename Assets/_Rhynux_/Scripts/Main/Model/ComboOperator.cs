
using UniRx;

public class ComboOperator {
	public ComboOperator (SessionManager _sessionManager, ReactiveReferee _reactiveReferee, RealtimeReferee _realtimeReferee) {
		_reactiveReferee.OnHit.Subscribe (x => {
			switch (x.Item2) {
				case AccuracyLevel.Perfect or AccuracyLevel.Good:
					_sessionManager.IncreaseCombo();
					break;
				case AccuracyLevel.Miss:
					_sessionManager.ResetCombo();
					break;
			}
		});

		_realtimeReferee.OnNoteStatusChanged.Subscribe (x => {
			switch (x.Item2) {
				case NoteAvailableStatus.Fell:
					_sessionManager.ResetCombo();
					break;
			}
		});
	}
}
