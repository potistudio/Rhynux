
using UniRx;

/// <summary>
/// Manage(Contain) the All of Information of Current Game Session.<br/>
/// exp. Score, Combo, Notes, Time, Chart etc...
/// </summary>
public class SessionManager {
	//* Notes
	private readonly System.Collections.Generic.List<Note> m_NotesCollection;
	public System.Collections.ObjectModel.ReadOnlyCollection<Note> NotesCollection => m_NotesCollection.AsReadOnly();

	// Emit Events when the Notes Are Enabled/Disabled
	private readonly Subject<Note> m_NoteDisabled = new();
	public System.IObservable<Note> OnNoteDisabled => OnNoteDisabled;

	private readonly Subject<Note> m_NoteEnabled = new();
	public System.IObservable<Note> OnNoteEnabled => OnNoteEnabled;

	public void SetNoteStatus (int _targetNoteIndex, NoteAvailableStatus _status) {
		m_NotesCollection[_targetNoteIndex].AvailableStatus = _status;

		if (_status == NoteAvailableStatus.Available)
			m_NoteEnabled.OnNext (m_NotesCollection[_targetNoteIndex]);

		if (_status == NoteAvailableStatus.Fell || _status == NoteAvailableStatus.Hit)
			m_NoteDisabled.OnNext (m_NotesCollection[_targetNoteIndex]);
	}

	//* Score
	private int m_CurrentScore = 0;
	public int CurrentScore => m_CurrentScore;
	public ReactiveProperty<int> CurrentScoreProperty => new (m_CurrentScore);

	public void AddScore (int _deltaScore) {
		m_CurrentScore += _deltaScore;
	}

	//* Time
	private float m_CurrentTime = 0f;
	public float CurrentTime => m_CurrentTime;
	public ReactiveProperty<float> OnTimeUpdated = new();

	public void UpdateTime (float _time) {
		m_CurrentTime = _time;
		OnTimeUpdated.Value = m_CurrentTime;
	}

	//* Combo
	private int m_CurrentCombo = 0;
	public int CurrentCombo => m_CurrentCombo;
	public ReactiveProperty<int> CurrentComboProperty => new (m_CurrentCombo);

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
		m_NotesCollection = _chart.Notes;
	}
}
