
using System.Linq;

public class ReactiveReferee {
	private readonly System.Collections.Generic.IReadOnlyList<Note> m_NotesList;
	private float m_CurrentTime;

	public ReactiveReferee (System.Collections.Generic.IReadOnlyList<Note> _notes) {
		m_NotesList = _notes;
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

				if (count == notesCountByLane - 1) {
					currentIndex = i;
					break;
				}

				minDistance = gap;
				preventIndex = i;

				count++;
			}
		}

		return (currentIndex, minDistance);
	}

	private AccuracyLevel Judge (float _distance) {
		return _distance switch {
			<= 60f => AccuracyLevel.Perfect,
			<= 120f => AccuracyLevel.Good,
			<= 160f => AccuracyLevel.Miss,
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
		float nearestNoteDistance = FindNearestNote (m_CurrentTime, _targetLane).Item2;
		AccuracyLevel accuracyLevel = Judge (nearestNoteDistance);

		return accuracyLevel;
	}
}
