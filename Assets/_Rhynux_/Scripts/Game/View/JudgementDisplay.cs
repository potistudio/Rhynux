
using MagicTween;
using UnityEngine;
using UniRx;

public class JudgementDisplay : MonoBehaviour {
	[SerializeField] private Transform m_TargetCanvas;
	[SerializeField] private GameObject m_PopupPrefab;

	public void ShowPopup (string _text) {
		Instantiate (m_PopupPrefab, m_TargetCanvas).GetComponent<Popup>().PopupText = _text;
	}
}
