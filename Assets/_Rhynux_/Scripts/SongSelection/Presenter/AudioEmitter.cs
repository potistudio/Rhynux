using UnityEngine;
using UniRx;

public sealed class AudioEmitter : MonoBehaviour {
	[SerializeField] private AudioClip m_TapSound;
	[SerializeField] private AudioSource m_AudioSource;

	[VContainer.Inject]
	private void Init (ScrollView _ScrollView) {
		_ScrollView.OnSelectionChange.Subscribe (_ => {
			m_AudioSource.PlayOneShot (m_TapSound);
		});
	}
}
