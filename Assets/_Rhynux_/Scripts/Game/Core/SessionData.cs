using System.Linq;

public class SessionData {
	private readonly System.Collections.ObjectModel.ReadOnlyCollection<Note> m_NotesCollection;

	public Note[] Notes { get { return m_NotesCollection.DeepCopy().ToArray(); }}

	public SessionData (System.Collections.Generic.IList<Note> _notes) {
		m_NotesCollection = _notes.ToList().AsReadOnly();
	}
}
