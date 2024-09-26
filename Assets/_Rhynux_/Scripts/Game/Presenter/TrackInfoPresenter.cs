public sealed class TrackInfoPresenter : VContainer.Unity.IStartable {
	SessionProxy m_Session;
	TrackInfoBanner m_TrackInfoBanner;

	public TrackInfoPresenter (SessionProxy _logic, TrackInfoBanner _view) {
		m_Session = _logic;
		m_TrackInfoBanner = _view;
	}

	public void Start() {
		Chart chart = m_Session.Session.Chart;
		m_TrackInfoBanner.SetLabel (chart.Title, chart.Artist);
	}
}
