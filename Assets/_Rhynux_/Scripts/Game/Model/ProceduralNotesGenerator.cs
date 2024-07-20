using System;
using System.Collections.Generic;

[System.Serializable]
public sealed class ProceduralNotesGenerator : INotesGenerator {
	public IObservable<IReadOnlyList<Note>> OnNotesGenerated => throw new NotImplementedException();

	public IList<Note> Generate (Chart _chart) {
		UnityEngine.Debug.Log ("Generated");
		return _chart.Notes;
	}
}
