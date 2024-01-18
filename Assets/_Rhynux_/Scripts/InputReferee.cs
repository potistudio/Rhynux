using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class InputReferee : MonoBehaviour {
	[SerializeField] private AudioSource m_AudioSource;
	private IInputInterface m_InputInterface;

	[VContainer.Inject]
	public void Init (IInputInterface _inputInterface, ReactiveNotesReferee _refree) {
		m_InputInterface = _inputInterface;
		m_InputInterface.OnPressed.Subscribe (_ => {
			_refree.FindNearestNote (m_AudioSource.time, _);
		}).AddTo (this);
	}
}
