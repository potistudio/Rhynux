using System;
using System.Collections.Generic;
using System.Linq;

[System.Serializable]
public sealed class ProceduralNotesGenerator : INotesGenerator {
	public IObservable<IReadOnlyList<Note>> OnNotesGenerated => throw new NotImplementedException();

	public IList<Note> Generate (Chart _chart) {
		UnityEngine.Debug.Log ("Generated");

		var n = _chart.Notes.Select (x => { var y = new Note((x.Time + _chart.Offset) * (60f / _chart.BPM), x.Position); return y; }).ToArray();
		return n;
	}
}
