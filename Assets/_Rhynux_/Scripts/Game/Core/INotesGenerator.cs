public interface INotesGenerator {
	System.Collections.Generic.IList<Note> Generate (Chart _chart);
	System.IObservable<System.Collections.Generic.IReadOnlyList<Note>> OnNotesGenerated { get; }
}
