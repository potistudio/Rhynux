using System.Linq;

namespace Rhynux {
	public class Chart {
		public Chart (string _title, string _artist, float _bpm, float _offset, SoundTrack _soundTrack, System.Collections.Generic.IList<Note> _notes, UnityEngine.Sprite _artwork = null, bool _secured = false) {
			m_Title = _title;
			m_Artist = _artist;
			m_BPM = _bpm;
			m_Offset = _offset;
			m_Notes = _notes.ToList();
			m_SoundTrack = _soundTrack;
			m_Artwork = _artwork;
			m_Secured = _secured;
		}

		private string m_Title;
		private string m_Artist;
		private float m_BPM;
		private float m_Offset;
		private SoundTrack m_SoundTrack;
		private System.Collections.Generic.List<Note> m_Notes;
		private UnityEngine.Sprite m_Artwork;
		private bool m_Secured;

		public string Title => m_Title;
		public string Artist => m_Artist;
		public float BPM => m_BPM;
		public float Offset => m_Offset;
		public SoundTrack Track { get => m_SoundTrack; set => m_SoundTrack = value; }
		public System.Collections.Generic.IReadOnlyList<Note> Notes => m_Notes;
		public UnityEngine.Sprite Artwork => m_Artwork;
		public bool Secured => m_Secured;

		//* Authoring metadata. Set through an object initializer so the constructor
		//* signature stays manageable; ChartAsset is the only producer.
		public ChartDifficulty Difficulty { get; init; }
		public int DifficultyLevel { get; init; }
		public string Charter { get; init; }
	}
}
