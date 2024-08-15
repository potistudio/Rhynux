public class TrackInfoBanner : UnityEngine.MonoBehaviour {
	//FIXME: Attach from Entry Point (not Inspector)
	[UnityEngine.SerializeField] private ChartAsset m_ChartAsset;

	[UnityEngine.SerializeField] private TMPro.TextMeshProUGUI m_TitleTextField;
	[UnityEngine.SerializeField] private TMPro.TextMeshProUGUI m_ArtistTextField;

	//FIXME: Execute from Entry Point (Unity Start)
	private void Start() {
		m_TitleTextField.text = m_ChartAsset.Chart.Title;
		m_ArtistTextField.text = m_ChartAsset.Chart.Artist;
	}
}
