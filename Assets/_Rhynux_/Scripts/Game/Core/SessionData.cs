using System.Linq;
// Abstract Factory
//  ↓ generate session data
// Facade
// Proxy
namespace Rhynux.Game {
	public sealed class SessionData {
		private readonly Chart m_Chart;
		private readonly System.Collections.ObjectModel.ReadOnlyCollection<Note> m_NotesCollection;

		public Chart Chart => m_Chart;

		/// <summary>
		/// The notes of this session, ordered by time.
		/// The collection is copied once on construction and shared from then on:
		/// referees and views read this every frame, so it must not allocate.
		/// </summary>
		public System.Collections.Generic.IReadOnlyList<Note> Notes => m_NotesCollection;

		public SessionData (Chart _chart, System.Collections.Generic.IList<Note> _notes) {
			m_Chart = _chart;
			m_NotesCollection = _notes.ToList().AsReadOnly();
		}
	}
}
