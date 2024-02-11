
using System.Linq;

public class RealtimeReferee {
	private readonly System.Collections.ObjectModel.ReadOnlyCollection<Queue> m_NotesCollection;

	public RealtimeReferee (System.Collections.Generic.IReadOnlyList<Note> _notes) {
		m_NotesCollection = _notes.Select (x => new Queue(x)).ToList().AsReadOnly();
		// var a = (System.Collections.Generic.List<Queue>)m_NotesCollection; ← Cannot cast
		// m_NotesCollection[0] = new Queue(_notes[0]); ← Cannot ReAssign
	}

	public void UpdateTime (float _targetTime) {
		foreach (Queue queue in m_NotesCollection) {
			if (queue.Note.Time <= _targetTime)
				queue.Fall();
		}
	}

	public class Queue {
		//TODO: Note to struct
		public Note Note { get; }
		public bool Fallen { get; private set; }

		public Queue (Note _note) {
			Note = _note;
			Fallen = false;
		}

		public void Fall() {
			Fallen = true;
		}
	}
}
