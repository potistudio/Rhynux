public abstract class ChartAsset : UnityEngine.ScriptableObject {
	[UnityEngine.SerializeField] private ChartDifficulty m_Difficulty;
	[UnityEngine.SerializeField] private int m_ChartDifficultyLevel;
	[UnityEngine.SerializeField] private string m_Charter;

	public virtual Chart Chart { get; }
}
