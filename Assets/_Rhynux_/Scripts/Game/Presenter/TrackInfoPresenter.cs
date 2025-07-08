public sealed class TrackInfoPresenter : VContainer.Unity.IStartable {
	SessionFactory m_Session;
	TrackInfoBanner m_TrackInfoBanner;

	public TrackInfoPresenter (SessionFactory _logic, TrackInfoBanner _view) {
		m_Session = _logic;
		m_TrackInfoBanner = _view;
	}

	public void Start() {
		Chart chart = m_Session.SessionPool.Chart;
		m_TrackInfoBanner.SetLabel (chart.Title, chart.Artists[0]);
	}
}
