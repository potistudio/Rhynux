using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "Json Chart", menuName = "Rhynux/Json Chart")]
public sealed class JsonChartAsset : ChartAsset {
	[SerializeField] private TextAsset m_JsonChartText;
	[SerializeField] private AudioClip m_SongClip;
	[SerializeField] private bool m_Secured;

	public override Chart Chart {
		get {
			JsonChart jsonChart = JsonUtility.FromJson<JsonChart>(m_JsonChartText.text);
			System.Collections.Generic.IList<Note> notes;

			notes = jsonChart.notes.Select (x => {
				Note result;
				float time = (float)x.num / (float)x.LPB;
				int lane = x.block;

				if (x.type == 1) {
					result = new TapNote (time, lane);
				} else if (x.type == 2) {
					JsonSecondNote[] childNotes = x.notes;
					float endTime = (float)childNotes[childNotes.Length - 1].num / (float)childNotes[childNotes.Length - 1].LPB;

					result = new HoldNote (time, lane, 2f);
				} else {
					throw new System.Exception ("Unknown Note Type: " + x.type);
				}

				return result;
			}).ToArray();

			return new Chart (jsonChart.name, "Unknown", jsonChart.BPM, jsonChart.offset * 0.0001f, new SoundTrack(m_SongClip), notes, m_Secured);
		}
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
		public JsonSecondNote[] notes;
		public int num;
		public int type;
	}

	[System.Serializable]
	private class JsonSecondNote {
		public int block;
		public int LPB;
		public int num;
		public int type;
	}
}
