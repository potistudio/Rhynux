using System.Linq;
using UnityEngine;

public class Chart {
	public Chart (string _title, string _artist, float _bpm, float _offset, AudioClip _clip, System.Collections.Generic.IList<Note> _notes) {
		m_Title = _title;
		m_Artist = _artist;
		m_BPM = _bpm;
		m_Offset = _offset;
		m_Clip = _clip;
		m_Notes = _notes.ToList();
	}

	[SerializeField] private string m_Title;
	[SerializeField] private string m_Artist;
	[SerializeField] private float m_BPM;
	[SerializeField] private float m_Offset;
	[SerializeField] private AudioClip m_Clip;
	[SerializeField] private System.Collections.Generic.List<Note> m_Notes;

	public string Title => m_Title;
	public string Artist => m_Artist;
	public float BPM => m_BPM;
	public float Offset => m_Offset;
	public AudioClip Clip => m_Clip;
	public System.Collections.Generic.IList<Note> Notes => m_Notes.DeepCopy();
}
