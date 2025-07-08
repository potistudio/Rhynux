using System.Linq;

// Immutable chart data class
// This class represents a chart in the game, containing metadata about the chart
public sealed class Chart {
	private System.Guid m_Id;

	private string m_Title;
	private string[] m_Artists;
	private string[] m_Charters;

	private int m_Difficulty;
	private bool m_Secured;

	private float m_BPM;
	private float m_Offset;

	private SoundTrack m_SoundTrack;
	private UnityEngine.Sprite m_Artwork;

	private System.Collections.Generic.List<Note> m_Notes;


	public System.Guid Id => m_Id;

	public string Title => m_Title;
	public string[] Artists => m_Artists;

	public float BPM => m_BPM;
	public float Offset => m_Offset;
	public SoundTrack Track { get => m_SoundTrack; set => m_SoundTrack = value; }
	public System.Collections.Generic.IList<Note> Notes => m_Notes.DeepCopy();
	public UnityEngine.Sprite Artwork => m_Artwork;
	public bool Secured => m_Secured;


	public Chart(string _title, string[] _artists, float _bpm, float _offset, SoundTrack _soundTrack, System.Collections.Generic.IList<Note> _notes, UnityEngine.Sprite _artwork = null, bool _secured = false) {
		m_Id = System.Guid.NewGuid();

		m_Title = _title;
		m_Artists = _artists;
		m_BPM = _bpm;
		m_Offset = _offset;
		m_Notes = _notes.ToList();
		m_SoundTrack = _soundTrack;
		m_Artwork = _artwork;
		m_Secured = _secured;
	}
}
