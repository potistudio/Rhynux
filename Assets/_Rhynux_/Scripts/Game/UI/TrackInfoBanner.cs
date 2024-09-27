public sealed class TrackInfoBanner : UnityEngine.MonoBehaviour {
	[UnityEngine.SerializeField] private TMPro.TextMeshProUGUI m_TitleTextField;
	[UnityEngine.SerializeField] private TMPro.TextMeshProUGUI m_ArtistTextField;

	public void SetLabel (string _title, string _artist) {
		m_TitleTextField.text = _title;
		m_ArtistTextField.text = _artist;
	}
}
