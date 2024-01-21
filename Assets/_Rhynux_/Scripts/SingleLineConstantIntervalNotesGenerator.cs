
using System.Collections.Generic;

[System.Serializable]
public class SingleLineConstantIntervalNotesGenerator : INotesGenerator {
	[UnityEngine.SerializeField] private int m_TargetLine;

	private readonly UniRx.Subject<IReadOnlyList<Note>> m_OnNotesGenerated = new();
	public System.IObservable<IReadOnlyList<Note>> OnNotesGenerated => m_OnNotesGenerated;

	public List<Note> Generate (Chart _chart) {
		List<Note> notes = new();

		float length = _chart.m_Clip.length;
		float currentTime = 0f;
		float interval = 60f / _chart.m_BPM;

		while (currentTime <= length) {
			currentTime += interval;
			notes.Add (new Note(currentTime, m_TargetLine));
		}

		m_OnNotesGenerated.OnNext (notes);
		return notes;
	}
}
