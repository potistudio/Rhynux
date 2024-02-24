
[System.Serializable]
public class Note {
	[UnityEngine.SerializeField] private float m_Time;
	[UnityEngine.SerializeField] private int m_Position;
	private NoteAvailableStatus m_AvailableStatus;

	public float Time { get { return m_Time; } set { m_Time = value; }}
	public int Position { get { return m_Position; } set { m_Position = value; }}
	public NoteAvailableStatus AvailableStatus { get { return m_AvailableStatus; } set { m_AvailableStatus = value; }}

	public Note (float _time, int _pos) {
		m_Time = _time;
		m_Position = _pos;
	}
}
