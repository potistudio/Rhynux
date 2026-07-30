using Cysharp.Threading.Tasks;

public sealed class AutoInputHandler : IInputHandler, VContainer.Unity.ITickable {
	private const int RELEASE_DELAY_MS = 40;

	private readonly MusicPlayer m_MusicPlayer;
	private readonly SessionFactory m_Session;

	private readonly UniRx.Subject<int> m_Pressed = new();
	private readonly UniRx.Subject<int> m_Released = new();

	private int m_CurrentIndex = 0;

	// The container ticks this handler unconditionally, but it must only drive
	// playback once InputHandlerFactory has actually selected auto mode.
	private bool m_IsActive = false;

	public System.IObservable<int> OnPressed => m_Pressed;
	public System.IObservable<int> OnReleased => m_Released;

	public AutoInputHandler (MusicPlayer _musicPlayer, SessionFactory _session) {
		m_MusicPlayer = _musicPlayer;
		m_Session = _session;
	}

	public void Activate() {
		m_IsActive = true;
	}

	private void Press (int _lane) {
		m_Pressed.OnNext (_lane);
	}

	private async UniTaskVoid WaitThenRelease (int _lane) {
		await UniTask.Delay (RELEASE_DELAY_MS);
		Release (_lane);
	}

	private void Release (int _lane) {
		m_Released.OnNext (_lane);
	}

	public void Tick() {
		if (!m_IsActive)
			return;

		System.Collections.Generic.IReadOnlyList<Note> notes = m_Session.SessionPool.Notes;
		float currentTime = m_MusicPlayer.CurrentTime;

		// Notes can share a timestamp (chords) or bunch up inside a single frame at high
		// BPM, so drain everything that is already due instead of one note per frame.
		while (m_CurrentIndex < notes.Count && currentTime >= notes[m_CurrentIndex].Time) {
			int lane = notes[m_CurrentIndex].Position;

			Press (lane);
			WaitThenRelease (lane).Forget();

			m_CurrentIndex++;
		}
	}
}
