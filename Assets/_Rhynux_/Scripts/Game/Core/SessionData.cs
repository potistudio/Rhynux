using System.Linq;
// Abstract Factory
//  ↓ generate session data
// Facade
// Proxy
public sealed class SessionData {
	private readonly Chart m_Chart;
	private readonly System.Collections.ObjectModel.ReadOnlyCollection<Note> m_NotesCollection;

	public Chart Chart { get { return m_Chart; }}
	public Note[] Notes { get { return m_NotesCollection.DeepCopy().ToArray(); }}

	public SessionData (Chart _chart, System.Collections.Generic.IList<Note> _notes) {
		m_Chart = _chart;
		m_NotesCollection = _notes.ToList().AsReadOnly();
	}
}
