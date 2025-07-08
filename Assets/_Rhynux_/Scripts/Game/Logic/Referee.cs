//* Refed from prpr
using System.Collections.Generic;
using System.Linq;

public class Referee {
	//TODO: Make this configurable
	private const int MAX_LANES = 4;  // Maximum number of lanes supported

	private const float LIMIT_PERFECT = 0.08f;
	private const float LIMIT_GOOD = 0.16f;
	private const float LIMIT_BAD = 0.22f;

	private const float DIST_FACTOR = 0.2f;

	// <(note, lane_index)>
	private List<List<Note>> m_Notes = new();
	private float m_LastTime = 0f;
	private JudgeInner m_JudgeInner;

	public Referee(Chart _chart) {
		//NOTE: sorted by time
		//* Separate the notes by each lane
		foreach (Note note in _chart.Notes) {
			int lane = note.Position;

			//* Prepare the lane list if it does not exist
			while (m_Notes.Count <= lane)
				m_Notes.Add(new List<Note>());
			//* --------

			m_Notes[lane].Add(note);
		}

		m_JudgeInner = new(_chart.Notes.Count);
		//* --------
	}

	public void Update(float _time) {
		const float SPEED = 1f;

		if (UnityEngine.Input.GetKeyDown(UnityEngine.KeyCode.D)) {
			// Detect lane position
			var position = UnityEngine.Input.mousePosition;
		}
	}
}
