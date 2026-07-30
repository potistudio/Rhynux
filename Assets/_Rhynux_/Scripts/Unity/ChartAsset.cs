using UnityEngine;

public abstract class ChartAsset : ScriptableObject {
	[SerializeField] protected ChartDifficulty m_Difficulty;
	[SerializeField] protected int m_ChartDifficultyLevel;
	[SerializeField] protected string m_Charter;

	[SerializeField] protected string m_Composer;
	[SerializeField] protected Sprite m_Artwork;

	public abstract Chart Unpack();
}
