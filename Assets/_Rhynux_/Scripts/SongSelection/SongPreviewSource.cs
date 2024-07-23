
using UnityEngine;
using UniRx;
using MagicTween;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class SongPreviewSource : MonoBehaviour {
	//= Serialized Field
	[SerializeField] private AudioSource m_AudioSource1;
	[SerializeField] private AudioSource m_AudioSource2;

	[SerializeField] private AudioClip m_PreviewClip1;
	[SerializeField] private AudioClip m_PreviewClip2;

	[SerializeField] private float m_CrossFadingTime;
	[SerializeField] private Ease m_Ease;

	//= Private Field
	private bool m_TFlipFlop = false;
	private AudioClip m_CurrentClip = default;
	private AudioSource m_CurrentAudioSource = null;

	private System.IDisposable m_LoopTimer;

	//= Property
	public AudioClip PreviewClip1 => m_PreviewClip1;
	public AudioClip PreviewClip2 => m_PreviewClip2;
	public AudioClip CurrentClip => m_CurrentClip;

	public bool TFlipFlop => m_TFlipFlop;

	//= Unity Method
	private void Start() {
		m_AudioSource1.volume = 0f;
		m_AudioSource2.volume = 0f;

		ChangePreviewClip (m_PreviewClip1);
	}

	//= Method
	public void ChangePreviewClip (AudioClip clip) {
		if (clip == null)
			return;

		m_TFlipFlop = !m_TFlipFlop; // Toggle

		//* FlipFlop
		AudioSource sourceTo = m_TFlipFlop ? m_AudioSource2 : m_AudioSource1;
		AudioSource sourceFrom = m_TFlipFlop ? m_AudioSource1 : m_AudioSource2;

		//* Play clip
		if (sourceTo.volume == 0f || sourceTo.clip != clip) {
			sourceTo.clip = clip;
			sourceTo.Play();
		}

		//* Looping
		m_LoopTimer?.Dispose();
		m_LoopTimer = Observable.Timer (System.TimeSpan.FromSeconds(clip.length - sourceTo.time - m_CrossFadingTime))
								.Subscribe (_ => ChangePreviewClip(clip))
								.AddTo (this);

		//* Fade out
		Tween.To (() => sourceFrom.volume, x => sourceFrom.volume = x, 0f, m_CrossFadingTime)
			.SetEase (m_Ease)
			.SetLink (sourceFrom.gameObject);

		//* Fade in
		Tween.To (() => sourceTo.volume, x => sourceTo.volume = x, 1f, m_CrossFadingTime)
			.SetEase (m_Ease)
			.SetLink (sourceTo.gameObject);

		m_CurrentClip = clip;
		m_CurrentAudioSource = sourceTo;
	}

	public void FadeOut (float _duration) {
		Tween.To (() => m_CurrentAudioSource.volume, x => m_CurrentAudioSource.volume = x, 0f, _duration)
			.SetEase (m_Ease)
			.SetLink (m_CurrentAudioSource.gameObject);
	}

	public void FadeIn (float _duration) {
		Tween.To (() => m_CurrentAudioSource.volume, x => m_CurrentAudioSource.volume = x, 1f, _duration)
			.SetEase (m_Ease)
			.SetLink (m_CurrentAudioSource.gameObject);
	}
}


#if UNITY_EDITOR

[CustomEditor(typeof(SongPreviewSource))]
public class SongPreviewSourceEditor : Editor {
	public override void OnInspectorGUI() {
		base.OnInspectorGUI();
		SongPreviewSource m_Target = target as SongPreviewSource;

		if (GUILayout.Button("Change Clip")) {
			m_Target.ChangePreviewClip (m_Target.TFlipFlop ? m_Target.PreviewClip1 : m_Target.PreviewClip2);
		}
	}
}

#endif
