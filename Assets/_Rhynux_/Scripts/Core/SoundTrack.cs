using UnityEngine;

/// <summary>
/// The sound track
/// </summary>
[System.Serializable]
public class SoundTrack {
	[SerializeField] AudioClip m_SoundClip;
	[SerializeField] private float m_DropTime;

	/// <summary>
	/// The sound clip
	/// </summary>
	public AudioClip SoundClip { get => m_SoundClip; set => m_SoundClip = value; }

	/// <summary>
	/// The time of the last drop marker
	/// </summary>
	public float DropTime { get => m_DropTime; set => m_DropTime = value; }

	/// <summary>
	/// The duration of this track
	/// </summary>
	public float Duration => SoundClip.length;

	/// <summary>
	/// Create a new SoundTrack
	/// </summary>
	/// <param name="_soundClip">The sound clip</param>
	public SoundTrack (UnityEngine.AudioClip _soundClip) {
		m_SoundClip = _soundClip;
		m_DropTime = 0f;
	}
}
