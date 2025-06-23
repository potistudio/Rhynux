using LitMotion;
using LitMotion.Extensions;

public class Popup : UnityEngine.MonoBehaviour {
	[UnityEngine.SerializeField] private TMPro.TextMeshProUGUI m_DisplayTextMesh;
	public string PopupText { get { return m_DisplayTextMesh.text; } set { m_DisplayTextMesh.text = value; }}

	private void Start() {
		LMotion.Create(0f, 2f, 1f)
			.WithEase(Ease.OutCubic)
			.WithOnComplete(() => Destroy(gameObject))
			.BindToLocalPositionY(transform);
	}
}
