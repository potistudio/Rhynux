public class TrackInfoBanner : UnityEngine.MonoBehaviour {
	[UnityEngine.SerializeField] private TMPro.TextMeshProUGUI m_TitleTextField;
	[UnityEngine.SerializeField] private TMPro.TextMeshProUGUI m_ArtistTextField;

	[VContainer.Inject]
	private void Init (Chart _chart) {
		m_TitleTextField.text = _chart.Title;
		m_ArtistTextField.text = _chart.Artist;
	}
}
