
using UnityEngine;
using UniRx;

public class JudgementDisplay : MonoBehaviour {
	[SerializeField] private Transform m_TargetCanvas;
	[SerializeField] private GameObject m_PopupPrefab;

	[VContainer.Inject]
	private void Init (ReactiveReferee _reactiveReferee) {
		_reactiveReferee.OnHit.Subscribe (_ => Debug.Log("Hit" + _));
	}

	public void ShowPopup (string _text) {
		Instantiate (m_PopupPrefab, m_TargetCanvas).GetComponent<Popup>().PopupText = _text;
	}
}
