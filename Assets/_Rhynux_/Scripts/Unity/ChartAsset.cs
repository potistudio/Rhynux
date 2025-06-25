using UnityEngine;

public abstract class ChartAsset : ScriptableObject {
	[SerializeField] private ChartDifficulty m_Difficulty;
	[SerializeField] private int m_ChartDifficultyLevel;
	[SerializeField] private string m_Charter;

	[SerializeField] protected string m_Composer;
	[SerializeField] protected Sprite m_Artwork;

	public abstract Chart Unpack();
}
