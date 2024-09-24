
[System.Serializable]
public class Note {
	[UnityEngine.SerializeField] private float m_Time;
	[UnityEngine.SerializeField] private int m_Position;
	protected NoteType m_NoteType;
	private NoteAvailableStatus m_AvailableStatus;

	public float Time { get { return m_Time; } set { m_Time = value; }}
	public int Position { get { return m_Position; } set { m_Position = value; }}
	public NoteType NoteType { get => m_NoteType; set => m_NoteType = value; }
	public NoteAvailableStatus AvailableStatus { get { return m_AvailableStatus; } set { m_AvailableStatus = value; }}

	public Note (float _time, int _pos) {
		m_Time = _time;
		m_Position = _pos;
	}
}

[System.Serializable]
public sealed class TapNote : Note {
	public TapNote (float _time, int _pos) : base (_time, _pos) {
		m_NoteType = NoteType.Tap;
	}
}

[System.Serializable]
public sealed class HoldNote : Note {
	private float m_Duration;

	public float Duration { get { return m_Duration; } }

	public HoldNote (float _time, int _pos, float _duration) : base (_time, _pos) {
		m_NoteType = NoteType.Hold;
		m_Duration = _duration;
	}
}

public enum NoteType {
	Tap,
	Hold,
	Slide
}
