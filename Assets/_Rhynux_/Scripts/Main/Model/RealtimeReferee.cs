
using System.Linq;

public class RealtimeReferee {
	private readonly System.Collections.ObjectModel.ReadOnlyCollection<Note> m_NotesCollection;
	private float m_LastUpdatedTime = 0f;

	private readonly UniRx.Subject<(int, bool)> m_NoteStatusChanged = new();
	public System.IObservable<(int, bool)> OnNoteStatusChanged => m_NoteStatusChanged;

	private readonly float m_Margin = 160f;

	public RealtimeReferee (System.Collections.Generic.IList<Note> _notes) {
		m_NotesCollection = _notes.ToList().AsReadOnly();
		// var a = (System.Collections.Generic.List<Queue>)m_NotesCollection; ← Cannot cast
		// m_NotesCollection[0] = new Queue(_notes[0]); ← Cannot ReAssign
	}

	public void UpdateTime (float _targetTime) {
		if (_targetTime == m_LastUpdatedTime)
			return;

		for (int i = 0; i < m_NotesCollection.Count; i++) {
			if (m_NotesCollection[i].Time < _targetTime + m_Margin)
				DisableNote (i);
			else if (m_NotesCollection[i].Time >= _targetTime + m_Margin)
				EnableNote (i);
		}

		m_LastUpdatedTime = _targetTime;
	}

	private void DisableNote (int _targetNoteIndex) {
		m_NotesCollection[_targetNoteIndex].AvailableStatus = NoteAvailableStatus.Fell;
		m_NoteStatusChanged.OnNext((_targetNoteIndex, false));
	}

	private void EnableNote (int _targetNoteIndex) {
		m_NotesCollection[_targetNoteIndex].AvailableStatus = NoteAvailableStatus.Available;
		m_NoteStatusChanged.OnNext((_targetNoteIndex, true));
	}
}
