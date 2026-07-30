using System;
using System.Collections.Generic;
using System.Linq;

namespace Rhynux.Game {
	[System.Serializable]
	public sealed class ProceduralNotesGenerator : INotesGenerator {
		private readonly UniRx.Subject<IReadOnlyList<Note>> m_OnNotesGenerated = new();
		public IObservable<IReadOnlyList<Note>> OnNotesGenerated => m_OnNotesGenerated;

		public IList<Note> Generate (Chart _chart) {
			float secondsPerBeat = 60f / _chart.BPM;

			// Note.Time is in beats, so it is the beat that scales by tempo.
			// The chart offset is already in seconds and must be added afterwards.
			Note[] notes = _chart.Notes
				.Select (x => new Note(x.Time * secondsPerBeat + _chart.Offset, x.Position))
				.ToArray();

			m_OnNotesGenerated.OnNext (notes);
			return notes;
		}
	}
}
