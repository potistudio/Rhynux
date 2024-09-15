
using System.Linq;

public class RealtimeReferee {
	private readonly UniRx.Subject<(int, NoteAvailableStatus)> m_NoteStatusChanged = new();
	public System.IObservable<(int, NoteAvailableStatus)> OnNoteStatusChanged => m_NoteStatusChanged;

	private readonly System.Collections.ObjectModel.ReadOnlyCollection<Note> m_NotesCollection;
	private readonly float m_Margin = 160f;

	private float m_CurrentTime = 0f;

	public RealtimeReferee (SessionData _session) {
		m_NotesCollection = _session.Notes.ToList().AsReadOnly();
	}

	public void UpdateTime (float _time) {
		int newIndex = FindBehindNote (_time);

		var targets = m_NotesCollection.Where ((x, i) => i <= newIndex).Select ((x, i) => (x, i));
		foreach ((Note x, int i) in targets) {
			FallNote (i);
		}

		m_CurrentTime = _time;
	}

	private int FindBehindNote (float _time) {
		var n = m_NotesCollection.Where (x => x.Time < _time);
		if (n.Count() == 0)
			return -1;

		return n.Select((x, i) => i).LastOrDefault();
	}

	private void FallNote (int _targetIndex) {
		m_NoteStatusChanged.OnNext ((_targetIndex, NoteAvailableStatus.Fell));
	}
}
