using System;
using System.Collections.Generic;
using System.Linq;

[System.Serializable]
public sealed class ProceduralNotesGenerator : INotesGenerator {
	public IObservable<IReadOnlyList<Note>> OnNotesGenerated => throw new NotImplementedException();

	public IList<Note> Generate (Chart _chart) {
		UnityEngine.Debug.Log ("Notes Generated with Procedural");

		float secondsPerBeat = 60f / _chart.BPM;

		// Note.Time is in beats, so it is the beat that scales by tempo.
		// The chart offset is already in seconds and must be added afterwards.
		return _chart.Notes
			.Select (x => new Note(x.Time * secondsPerBeat + _chart.Offset, x.Position))
			.ToArray();
	}
}
