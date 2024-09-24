using System;
using System.Collections.Generic;
using System.Linq;

[System.Serializable]
public sealed class ProceduralNotesGenerator : INotesGenerator {
	public IObservable<IReadOnlyList<Note>> OnNotesGenerated => throw new NotImplementedException();

	public IList<Note> Generate (Chart _chart) {
		UnityEngine.Debug.Log ("Notes Generated with Procedural");

		var n = _chart.Notes.Select (x => {
			Note note;

			if (x.NoteType == NoteType.Tap) {
				note = new TapNote ((x.Time + _chart.Offset) * (60f / _chart.BPM), x.Position);
			} else if (x.NoteType == NoteType.Hold) {
				note = new HoldNote ((x.Time + _chart.Offset) * (60f / _chart.BPM), x.Position, (x as HoldNote).Duration);
			} else {
				throw new Exception ("Unknown Note Type");
			}

			return note;
		}).ToArray();

		return n;
	}
}
