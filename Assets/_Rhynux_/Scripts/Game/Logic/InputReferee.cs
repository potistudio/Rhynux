namespace Rhynux.Game {
	public sealed class InputReferee {
		// private readonly System.Collections.Generic.IReadOnlyList<Note> m_NotesList;
		private readonly SessionFactory m_SessionFactory;
		private float m_CurrentTime;

		private UniRx.Subject<(int index, int lane, AccuracyLevel accuracy)> m_OnHit = new();
		public System.IObservable<(int index, int lane, AccuracyLevel accuracy)> OnHit => m_OnHit;

		public InputReferee (SessionFactory _session) {
			// m_NotesList = _session.Notes;
			m_SessionFactory = _session;
		}

		/// <summary>
		///	Find the nearest Note from Given Time (Current Time).
		/// </summary>
		/// <param name="_time">Time</param>
		/// <param name="_lane">Lane</param>
		/// <returns>(int Index, float Distance). Index is -1 when the lane holds no note.</returns>
		private (int index, float distance) FindNearestNote (float _time, int _lane) {
			System.Collections.Generic.IReadOnlyList<Note> notes = m_SessionFactory.SessionPool.Notes;

			int nearestIndex = -1;
			float minDistance = float.PositiveInfinity;

			for (int i = 0; i < notes.Count; i++) {
				if (notes[i].Position != _lane)
					continue;

				float gap = System.Math.Abs (_time - notes[i].Time);

				// Notes are ordered by time, so once the gap starts growing the nearest one is already behind us.
				if (gap > minDistance)
					break;

				minDistance = gap;
				nearestIndex = i;
			}

			return (nearestIndex, minDistance);
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
			(int nearestIndex, float nearestNoteDistance) = FindNearestNote (m_CurrentTime, _targetLane);
			AccuracyLevel accuracyLevel = Judge (nearestNoteDistance);

			// An empty lane yields an infinite distance, which Judge() maps to Pass.
			if (accuracyLevel != AccuracyLevel.Pass)
				m_OnHit.OnNext ((nearestIndex, _targetLane, accuracyLevel));

			return accuracyLevel;
		}
	}
}
