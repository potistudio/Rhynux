
using UniRx;

public class SessionTimeUpdater : UnityEngine.MonoBehaviour {
	[UnityEngine.SerializeField] private MusicPlayer m_MusicPlayer;

	private SessionManager m_SessionManager;

	[VContainer.Inject]
	private void Init (SessionManager _sessionManager) {
		m_SessionManager = _sessionManager;
	}

	private void Update() {
		// To millisecond
		m_SessionManager.UpdateTime (m_MusicPlayer.CurrentTime * 1000);
		UnityEngine.Debug.Log (m_SessionManager.NotesCollection[20].AvailableStatus);
	}
}
