//* Refer from prpr

public enum JudgeStatus {
	NotJudged,
	PreJudge,
	Judged,
	Hold
}

public class JudgeInner {
	private System.Collections.Generic.List<float> m_Diffs;
	private int m_Combo;
	private int m_MaxCombo;
	private int[] m_Counts;
	private int m_NotesCount;

	public int Combo => this.m_Combo;
	public int[] Counts => this.m_Counts;

	public JudgeInner(int _notesCount) {
		m_Diffs = new();
		m_Combo = 0;
		m_MaxCombo = 0;
		m_Counts = new int[4];
		m_NotesCount = _notesCount;
	}

	public void Reset() {
		this.m_Diffs.Clear();
		this.m_Combo = 0;
		this.m_MaxCombo = 0;
		this.m_Counts = new int[4];
	}

	public void Commit(AccuracyLevel _what, float _diff) {
		if (_what == AccuracyLevel.Good)
			this.m_Diffs.Add(_diff);

		switch (_what) {
			case AccuracyLevel.Perfect:
			case AccuracyLevel.Good:
				this.m_Combo++;

				if (this.m_Combo > this.m_MaxCombo)
					this.m_MaxCombo = this.m_Combo;

				break;
			default:
				this.m_Combo = 0;
				break;
		}
	}
}
