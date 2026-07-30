using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "Json Chart", menuName = "Rhynux/Json Chart")]
public sealed class JsonChartAsset : ChartAsset {
	[SerializeField] private TextAsset m_JsonChartText;
	[SerializeField] private AudioClip m_SongClip;
	[SerializeField] private bool m_Secured;

	public override Chart Unpack() {
		JsonChart jsonChart = JsonUtility.FromJson<JsonChart>(m_JsonChartText.text);
		System.Collections.Generic.IList<Note> notes;

		notes = jsonChart.notes.Select (x => {
			float time = (float)x.num / (float)x.LPB;
			int lane = x.block;

			return new Note (time, lane);
		}).ToArray();

		// The editor format stores the offset in milliseconds.
		const float MILLISECONDS_TO_SECONDS = 0.001f;

		return new Chart (jsonChart.name, m_Composer, jsonChart.BPM, jsonChart.offset * MILLISECONDS_TO_SECONDS, new SoundTrack(m_SongClip), notes, m_Artwork, m_Secured) {
			Difficulty = m_Difficulty,
			DifficultyLevel = m_ChartDifficultyLevel,
			Charter = m_Charter
		};
	}

	// JsonUtility writes these through reflection, which the compiler cannot see (CS0649).
	// It also only maps public fields, so private ones were silently never populated.
	#pragma warning disable 0649

	[System.Serializable]
	private class JsonChart {
		public float BPM;
		public string name;
		public JsonNote[] notes;
		public int offset;
	}

	[System.Serializable]
	private class JsonNote {
		public int block;
		public int LPB;
		public int num;
	}

	#pragma warning restore 0649
}
