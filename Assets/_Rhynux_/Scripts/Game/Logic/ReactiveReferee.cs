
using System;
using System.Linq;
using UniRx;

public class ReactiveReferee {
	private readonly System.Collections.Generic.IReadOnlyList<Note> m_NotesList;
	private float m_CurrentTime;

	private UniRx.Subject<(int index, int lane, AccuracyLevel accuracy)> m_OnHit = new();
	public System.IObservable<(int index, int lane, AccuracyLevel accuracy)> OnHit => m_OnHit;

	public ReactiveReferee (SessionData _session, InputHandlerFactory _factory) {
		m_NotesList = _session.Notes;

		var ih = _factory.CreateInputHandler (InputMode.Auto);
		ih.OnPressed.Subscribe (x => {
			JudgeHit (x);
		});
	}

	/// <summary>
	///	Find the nearest Note from Given Time (Current Time).
	/// </summary>
	/// <param name="time">Time</param>
	/// <param name="lane">Lane</param>
	/// <returns>(int Index, float Distance)</returns>
	private (int, float) FindNearestNote (float _time, int _lane) {
		int preventIndex = 0;
		int currentIndex = 0;

		int notesCountByLane = m_NotesList.Where (x => x.Position == _lane).Count();
		int count = 0;

		float minDistance = float.PositiveInfinity;

		for (int i = 0; i < m_NotesList.Count; i++) {
			if (m_NotesList[i].Position == _lane) {
				float gap = System.Math.Abs (_time - m_NotesList[i].Time);

				if (gap > minDistance) {
					currentIndex = preventIndex;
					break;
				}

				minDistance = gap;
				preventIndex = i;

				count++;
			}

		}

		if (count == notesCountByLane - 1) {
			currentIndex = count;
		}

		return (currentIndex, minDistance);
	}

	private AccuracyLevel Judge (float _distance) {
		return _distance switch {
			<= 0.060f => AccuracyLevel.Perfect,
			<= 0.120f => AccuracyLevel.Good,
			<= 0.160f => AccuracyLevel.Miss,
			_ => AccuracyLevel.Pass,
		};
	}

	/// <summary>
	/// Update Internal Time of this Referee.
	/// </summary>
	/// <param name="_targetTime">Target of Time to Shift</param>
	public void UpdateTime (float _targetTime) {
		m_CurrentTime = _targetTime;
	}

	public AccuracyLevel JudgeHit (int _targetLane) {
		(int a, float nearestNoteDistance) = FindNearestNote (m_CurrentTime, _targetLane);
		AccuracyLevel accuracyLevel = Judge (nearestNoteDistance);

		if (accuracyLevel != AccuracyLevel.Pass)
			m_OnHit.OnNext ((a, _targetLane, accuracyLevel));

		return accuracyLevel;
	}
}
