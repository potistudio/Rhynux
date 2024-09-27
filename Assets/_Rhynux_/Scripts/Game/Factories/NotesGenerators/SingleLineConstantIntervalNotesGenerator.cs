
using System.Collections.Generic;

[System.Serializable]
public class SingleLineConstantIntervalNotesGenerator : INotesGenerator {
	[UnityEngine.SerializeField] private int m_TargetLine;

	private readonly UniRx.Subject<IReadOnlyList<Note>> m_OnNotesGenerated = new();
	public System.IObservable<IReadOnlyList<Note>> OnNotesGenerated => m_OnNotesGenerated;

	public List<Note> GeneratedNotes { get; private set; }

	public IList<Note> Generate (Chart _chart) {
		UnityEngine.Debug.Log ("Notes Generated with Single");
		List<Note> notes = new();

		float length = _chart.Track.SoundClip.length;
		float currentTime = 0f;
		float interval = 60f / _chart.BPM;
		float offset = _chart.Offset * (60f / _chart.BPM);

		while (currentTime <= length) {
			notes.Add (new Note(currentTime + offset, m_TargetLine));
			currentTime += interval;
		}

		m_OnNotesGenerated.OnNext (notes);
		GeneratedNotes = notes;
		return notes;
	}
}
