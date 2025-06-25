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

		return new Chart (jsonChart.name, m_Composer, jsonChart.BPM, jsonChart.offset * 0.0001f, new SoundTrack(m_SongClip), notes, m_Artwork, m_Secured);
	}

	private class JsonChart {
		public float BPM;
		private int maxBlock;
		public string name;
		public JsonNote[] notes;
		public int offset;
	}

	[System.Serializable]
	private class JsonNote {
		public int block;
		public int LPB;
		private JsonNote[] notes;
		public int num;
		private int type;
	}
}
