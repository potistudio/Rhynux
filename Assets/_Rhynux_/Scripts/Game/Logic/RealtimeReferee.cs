
using System.Linq;

public class RealtimeReferee {
	private readonly System.Collections.ObjectModel.ReadOnlyCollection<Note> m_NotesCollection;
	private float m_CurrentUpdatedTime = 0f;

	private readonly UniRx.Subject<(int, NoteAvailableStatus)> m_NoteStatusChanged = new();
	public System.IObservable<(int, NoteAvailableStatus)> OnNoteStatusChanged => m_NoteStatusChanged;

	private readonly float m_Margin = 160f;

	public RealtimeReferee (System.Collections.Generic.IList<Note> _notes) {
		m_NotesCollection = _notes.DeepCopy().ToList().AsReadOnly();
		// var a = (System.Collections.Generic.List<Queue>)m_NotesCollection; ← Cannot cast
		// m_NotesCollection[0] = new Queue(_notes[0]); ← Cannot ReAssign
	}

	public void UpdateTime (float _targetTime) {
		if (_targetTime == m_CurrentUpdatedTime)
			return;

		for (int i = 0; i < m_NotesCollection.Count; i++) {
			if (m_NotesCollection[i].Time < _targetTime - m_Margin)
				DisableNote (i);
			else if (m_NotesCollection[i].Time >= _targetTime - m_Margin)
				EnableNote (i);
		}

		m_CurrentUpdatedTime = _targetTime;
	}

	private void DisableNote (int _targetNoteIndex) {
		// If Target Note was already Fallen, Ignore
		if (m_NotesCollection[_targetNoteIndex].AvailableStatus == NoteAvailableStatus.Fell)
			return;

		m_NotesCollection[_targetNoteIndex].AvailableStatus = NoteAvailableStatus.Fell;
		m_NoteStatusChanged.OnNext((_targetNoteIndex, NoteAvailableStatus.Fell));
	}

	private void EnableNote (int _targetNoteIndex) {
		// If Target Note was already Fallen, Ignore
		if (m_NotesCollection[_targetNoteIndex].AvailableStatus == NoteAvailableStatus.Available)
			return;

		m_NotesCollection[_targetNoteIndex].AvailableStatus = NoteAvailableStatus.Available;
		m_NoteStatusChanged.OnNext((_targetNoteIndex, NoteAvailableStatus.Available));
	}
}
