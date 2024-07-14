
using UniRx;

/// <summary>
/// Observe Referee Activity and Update Combo
/// </summary>
public class ComboOperator {
	public ComboOperator (SessionManager _sessionManager, NotesReferee _notesReferee) {
		_notesReferee.OnHit.Subscribe (x => {
			switch (x) {
				case AccuracyLevel.Perfect or AccuracyLevel.Good:
					_sessionManager.IncreaseCombo();
					break;
				case AccuracyLevel.Miss:
					_sessionManager.ResetCombo();
					break;
			}
		});

		_notesReferee.OnFall.Subscribe (x => {
			_sessionManager.ResetCombo();
		});
	}
}
