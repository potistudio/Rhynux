
using System.Linq;

public class RealtimeReferee {
	private readonly System.Collections.ObjectModel.ReadOnlyCollection<Queue> m_NotesCollection;
	private float m_LastUpdatedTime = 0f;

	private readonly UniRx.Subject<(int, bool)> m_NoteStatusChanged = new();
	public System.IObservable<(int, bool)> OnNoteStatusChanged => m_NoteStatusChanged;

	public RealtimeReferee (System.Collections.Generic.IReadOnlyList<Note> _notes) {
		m_NotesCollection = _notes.Select (x => new Queue(x)).ToList().AsReadOnly();
		// var a = (System.Collections.Generic.List<Queue>)m_NotesCollection; ← Cannot cast
		// m_NotesCollection[0] = new Queue(_notes[0]); ← Cannot ReAssign
	}

	public void UpdateTime (float _targetTime) {
		if (_targetTime == m_LastUpdatedTime)
			return;

		m_NotesCollection.Select ((x, i) => {
			if (x.Note.Time < _targetTime)
				DisableNote (i);
			else if (x.Note.Time > _targetTime)
				EnableNote (i);

			return x;
		});

		m_LastUpdatedTime = _targetTime;
	}

	private void DisableNote (int _targetNoteIndex) {
		m_NotesCollection[_targetNoteIndex].Enabled = false;
		m_NoteStatusChanged.OnNext((_targetNoteIndex, false));
	}

	private void EnableNote (int _targetNoteIndex) {
		m_NotesCollection[_targetNoteIndex].Enabled = true;
		m_NoteStatusChanged.OnNext((_targetNoteIndex, true));
	}

	private class Queue {
		public Note Note { get; }
		public bool Enabled { get; set; }

		public Queue (Note _note) {
			Note = _note;
			Enabled = true;
		}
	}
}
