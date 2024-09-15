public sealed class SessionTimeUpdater : VContainer.Unity.ITickable {
	private readonly MusicPlayer m_MusicPlayer;
	private readonly RealtimeReferee m_RealtimeReferee;
	private readonly ReactiveReferee m_ReactiveReferee;

	public SessionTimeUpdater (MusicPlayer _musicPlayer, RealtimeReferee _realtimeReferee, ReactiveReferee _reactiveReferee) {
		m_MusicPlayer = _musicPlayer;
		m_RealtimeReferee = _realtimeReferee;
		m_ReactiveReferee = _reactiveReferee;
	}

	public void Tick() {
		m_RealtimeReferee.UpdateTime (m_MusicPlayer.CurrentTime);
		m_ReactiveReferee.UpdateTime (m_MusicPlayer.CurrentTime);
	}
}
