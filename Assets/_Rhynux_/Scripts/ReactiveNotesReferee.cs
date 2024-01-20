
using System.Linq;

/* Model */
public class ReactiveNotesReferee {
	private readonly System.Collections.Generic.IReadOnlyList<Note> m_NotesList;

	public ReactiveNotesReferee (System.Collections.Generic.IReadOnlyList<Note> _notes) {
		m_NotesList = _notes;
	}

	public int FindNearestNote (float _time, int _lane) {
		int preventIndex = 0;
		int currentIndex = 0;

		int notesCountByLane = m_NotesList.Where(x => x.Position == _lane).Count();
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

		return currentIndex;
	}
}
