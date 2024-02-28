
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
	public System.IObservable<Note> OnNoteDisabled => m_NoteDisabled;

	private readonly Subject<Note> m_NoteEnabled = new();
	public System.IObservable<Note> OnNoteEnabled => m_NoteEnabled;

	public void SetNoteStatus (int _targetNoteIndex, NoteAvailableStatus _status) {
		if (m_NotesCollection[_targetNoteIndex].AvailableStatus == _status)
			return;

		m_NotesCollection[_targetNoteIndex].AvailableStatus = _status;

		if (_status == NoteAvailableStatus.Available)
			m_NoteEnabled.OnNext (m_NotesCollection[_targetNoteIndex]);

		if (_status == NoteAvailableStatus.Fell || _status == NoteAvailableStatus.Hit)
			m_NoteDisabled.OnNext (m_NotesCollection[_targetNoteIndex]);
	}

	//* Score
	private ReactiveProperty<int> m_CurrentScore = new();
	public ReadOnlyReactiveProperty<int> CurrentScore => m_CurrentScore.ToReadOnlyReactiveProperty();

	public void AddScore (int _deltaScore) {
		m_CurrentScore.Value += _deltaScore;
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
	private ReactiveProperty<int> m_CurrentCombo => new();
	public ReadOnlyReactiveProperty<int> CurrentCombo => m_CurrentCombo.ToReadOnlyReactiveProperty();

	public void IncreaseCombo() {
		m_CurrentCombo.Value++;
	}

	public void ResetCombo() {
		m_CurrentCombo.Value = 0;
	}

	//* Chart
	private Chart m_CurrentChart = default;
	public Chart CurrentChart => m_CurrentChart;

	private void SetChart (Chart _chart) {
		m_CurrentChart = _chart;
	}

	// etc.

	/// <summary>
	///
	/// </summary>
	/// <param name="chart">Chart</param>
	public SessionManager (Chart _chart) {
		SetChart (_chart);
		m_NotesCollection = _chart.Notes;
	}

	/// <summary>
	///
	/// </summary>
	/// <param name="chart">Chart</param>
	/// <param name="notes">List of Notes</param>
	public SessionManager (Chart _chart, System.Collections.Generic.List<Note> _notes) {
		SetChart (_chart);
		m_NotesCollection = _notes;
	}
}
