public sealed class RealtimeReferee {
	private readonly UniRx.Subject<(int, NoteAvailableStatus)> m_NoteStatusChanged = new();
	public System.IObservable<(int, NoteAvailableStatus)> OnNoteStatusChanged => m_NoteStatusChanged;

	private const float MARGIN = 0.160f;

	private float m_CurrentTime = 0f;
	private int m_NextNoteIndex = 0;
	private readonly SessionFactory m_SessionFactory;

	public RealtimeReferee (SessionFactory _session) {
		m_SessionFactory = _session;
	}

	public void UpdateTime (float _time) {
		System.Collections.Generic.IReadOnlyList<Note> notes = m_SessionFactory.SessionPool.Notes;

		// Seeking backwards replays the chart from the start, so the cursor has to follow.
		if (_time < m_CurrentTime)
			m_NextNoteIndex = 0;

		// Notes are ordered by time, so only the ones at the cursor can newly fall.
		// Walking from the cursor keeps this O(notes that fell this frame) instead of
		// re-emitting every fallen note on every single frame.
		while (m_NextNoteIndex < notes.Count && notes[m_NextNoteIndex].Time + MARGIN < _time) {
			FallNote (m_NextNoteIndex);
			m_NextNoteIndex++;
		}

		m_CurrentTime = _time;
	}

	private void FallNote (int _targetIndex) {
		m_NoteStatusChanged.OnNext ((_targetIndex, NoteAvailableStatus.Fell));
	}
}
