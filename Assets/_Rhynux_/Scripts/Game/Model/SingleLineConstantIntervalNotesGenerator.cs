
using System.Collections.Generic;

[System.Serializable]
public class SingleLineConstantIntervalNotesGenerator : INotesGenerator {
	[UnityEngine.SerializeField] private int m_TargetLine;

	private readonly UniRx.Subject<IReadOnlyList<Note>> m_OnNotesGenerated = new();
	public System.IObservable<IReadOnlyList<Note>> OnNotesGenerated => m_OnNotesGenerated;

	public List<Note> GeneratedNotes { get; private set; }

	public IList<Note> Generate (Chart _chart) {
		List<Note> notes = new();

		// "s" to "ms" by multiplying 1000
		float length = _chart.Clip.length * 1000;
		float currentTime = 0f;
		float interval = 60f / _chart.BPM * 1000;

		while (currentTime <= length) {
			currentTime += interval;
			notes.Add (new Note(currentTime, m_TargetLine));
		}

		m_OnNotesGenerated.OnNext (notes);
		GeneratedNotes = notes;
		return notes;
	}
}
