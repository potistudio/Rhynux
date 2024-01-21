
public interface INotesGenerator {
	System.Collections.Generic.List<Note> Generate (Chart _chart);
	System.IObservable<System.Collections.Generic.IReadOnlyList<Note>> OnNotesGenerated { get; }
}
