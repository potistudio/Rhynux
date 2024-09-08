using UniRx;

public class SessionTimeUpdater : VContainer.Unity.ITickable {
	private ReactiveReferee m_ReactiveReferee;
	private MusicPlayer m_MusicPlayer;

	private SessionTimeUpdater (ReactiveReferee _reactiveReferee, MusicPlayer _musicPlayer) {
		m_ReactiveReferee = _reactiveReferee;
		m_MusicPlayer = _musicPlayer;
	}

	public void Tick() {
		m_ReactiveReferee.UpdateTime (m_MusicPlayer.CurrentTime);
	}
}
