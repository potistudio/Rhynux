
public class SessionManager {
	//* Notes
	private System.Collections.Generic.List<Note> m_NotesCollection;
	public System.Collections.ObjectModel.ReadOnlyCollection<Note> NotesCollection => m_NotesCollection.AsReadOnly();

	private void SetNotes (System.Collections.Generic.List<Note> _notes) {
		m_NotesCollection = _notes;
	}

	//* Score
	private int m_CurrentScore = 0;
	public int CurrentScore => m_CurrentScore;

	public void AddScore (int _deltaScore) {
		m_CurrentScore += _deltaScore;
	}

	//* Time
	private float m_CurrentTime = 0f;
	public float CurrentTime => m_CurrentTime;

	public void UpdateTime (float _time) {
		m_CurrentTime = _time;
	}

	//* Combo
	private int m_CurrentCombo = 0;
	public int CurrentCombo => m_CurrentCombo;

	public void IncreaseCombo() {
		m_CurrentCombo++;
	}

	public void ResetCombo() {
		m_CurrentCombo = 0;
	}

	//* Chart
	private Chart m_CurrentChart = default;
	public Chart CurrentChart => m_CurrentChart;

	private void SetChart (Chart _chart) {
		m_CurrentChart = _chart;
	}

	// etc.

	public SessionManager (Chart _chart) {
		SetChart (_chart);
		SetNotes (_chart.Notes);
	}
}
