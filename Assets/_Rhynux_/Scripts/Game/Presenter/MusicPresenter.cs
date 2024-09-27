public sealed class MusicPresenter : VContainer.Unity.IStartable {
	private SessionFactory m_Session;
	private MusicPlayer m_MusicPlayer;

	[VContainer.Inject]
	private void Inject (SessionFactory _session, MusicPlayer _view) {
		m_Session = _session;
		m_MusicPlayer = _view;
	}

	public void Start() {
		m_MusicPlayer.Clip = m_Session.SessionPool.Chart.Track.SoundClip;
		m_MusicPlayer.Play();
	}
}
