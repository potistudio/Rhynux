
using UnityEngine;
using Alchemy.Inspector;

[CreateAssetMenu(fileName = "Chart", menuName = "Rhynux/Chart")]
public class Chart : ScriptableObject {
	public string m_Title;
	public string Artist;
	public float m_BPM;
	[LabelText("Offset (ms)")] public float m_Offset;

	public AudioClip m_Clip;

	public System.Collections.Generic.List<Note> m_Notes;
}

[System.Serializable]
public class Note {
	[field: SerializeField] public float Time { get; set; }
	[field: SerializeField] public float Position { get; set; }

	public Note (float _time, float _pos) {
		Time = _time;
		Position = _pos;
	}
}
