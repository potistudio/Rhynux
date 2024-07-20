
using UnityEngine;

[CreateAssetMenu(fileName = "Chart", menuName = "Rhynux/Chart")]
public class Chart : ScriptableObject {
	[SerializeField] private string m_Title;
	[SerializeField] private string m_Artist;
	[SerializeField] private float m_BPM;
	[SerializeField, Alchemy.Inspector.LabelText("Offset (ms)")] private float m_Offset;
	[SerializeField] private AudioClip m_Clip;
	[SerializeField] private System.Collections.Generic.IList<Note> m_Notes;

	public string Title => m_Title;
	public string Artist => m_Artist;
	public float BPM => m_BPM;
	public float Offset => m_Offset;
	public AudioClip Clip => m_Clip;
	public System.Collections.Generic.IList<Note> Notes => m_Notes.DeepCopy();
}
