namespace Rhynux.Game {
	public sealed class MusicPresenter : VContainer.Unity.IStartable {
		private SessionFactory m_Session;
		private MusicPlayer m_MusicPlayer;

		[VContainer.Inject]
		private void Inject (SessionFactory _session, MusicPlayer _view) {
			m_Session = _session;
			m_MusicPlayer = _view;
		}

		public void Start() {
			SoundTrack track = m_Session.SessionPool.Chart.Track;

			// Procedurally generated charts carry no audio, so guard instead of throwing.
			if (track?.SoundClip == null) {
				UnityEngine.Debug.LogWarning ("Chart has no sound clip; skipping playback.");
				return;
			}

			m_MusicPlayer.Clip = track.SoundClip;
			m_MusicPlayer.Play();
		}
	}
}
