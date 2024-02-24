
using MagicTween;

public class Popup : UnityEngine.MonoBehaviour {
	[UnityEngine.SerializeField] private TMPro.TextMeshProUGUI m_DisplayTextMesh;
	public string PopupText { get { return m_DisplayTextMesh.text; } set { m_DisplayTextMesh.text = value; }}

	private void Start() {
		transform.TweenLocalPositionY (2f, 1f).SetEase (Ease.OutCubic).OnComplete(() => Destroy(gameObject));
	}
}
