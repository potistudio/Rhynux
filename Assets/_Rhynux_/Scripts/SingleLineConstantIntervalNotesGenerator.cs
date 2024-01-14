
[System.Serializable]
public class SingleLineConstantIntervalNotesGenerator : INotesGenerator {
	[UnityEngine.SerializeField] private int m_TargetLine;

	public System.Collections.Generic.List<Note> Generate (Chart _chart) {
		System.Collections.Generic.List<Note> notes = new();

		float length = _chart.m_Clip.length;
		float currentTime = 0f;
		float interval = 60f / _chart.m_BPM;

		while (currentTime <= length) {
			currentTime += interval;
			notes.Add (new Note(currentTime, m_TargetLine));
		}

		return notes;
	}
}
