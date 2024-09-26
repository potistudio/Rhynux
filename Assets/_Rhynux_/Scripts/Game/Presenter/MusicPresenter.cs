public sealed class MusicPresenter : VContainer.Unity.IStartable {
	private SessionProxy m_Session;
	private MusicPlayer m_MusicPlayer;

	[VContainer.Inject]
	private void Inject (SessionProxy _session, MusicPlayer _view) {
		m_Session = _session;
		m_MusicPlayer = _view;
	}

	public void Start() {
		m_MusicPlayer.Clip = m_Session.Session.Chart.Track.SoundClip;
		m_MusicPlayer.Play();
	}
}
