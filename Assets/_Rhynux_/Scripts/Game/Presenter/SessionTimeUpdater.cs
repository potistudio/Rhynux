public sealed class SessionTimeUpdater : VContainer.Unity.ITickable {
	private readonly MusicPlayer m_MusicPlayer;
	private readonly RefereeFacade m_Referee;

	public SessionTimeUpdater (MusicPlayer _musicPlayer, RefereeFacade _referee) {
		m_MusicPlayer = _musicPlayer;
		m_Referee = _referee;
	}

	public void Tick() {
		m_Referee.UpdateTime (m_MusicPlayer.CurrentTime);
	}
}
