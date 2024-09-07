[System.Serializable]
public class AutoInputHandler : IInputHandler, VContainer.Unity.ITickable {
	private readonly UniRx.Subject<int> m_Pressed = new();
	public System.IObservable<int> OnPressed => m_Pressed;

	private readonly UniRx.Subject<int> m_Released = new();
	public System.IObservable<int> OnReleased => m_Released;

	private MusicPlayer m_MusicPlayer;

	private int m_CurrentIndex = 0;
	private Note[] notes;

	public AutoInputHandler (MusicPlayer _musicPlayer, SessionData _session, Chart _chart) : base() {
		m_MusicPlayer = _musicPlayer;
		notes = _session.Notes;
	}

	private void Press (int _lane) {
		m_Pressed.OnNext (_lane);
	}

	private void Release (int _lane) {
		m_Released.OnNext (_lane);
	}

	public void Tick() {
		if (m_CurrentIndex >= notes.Length) {
			UnityEngine.Debug.Log ("End");
			return;
		}

		if (m_MusicPlayer.CurrentTime >= notes[m_CurrentIndex].Time) {
			Press (notes[m_CurrentIndex].Position);
		}

		if (m_MusicPlayer.CurrentTime >= notes[m_CurrentIndex].Time + 0.08f) {
			Release (notes[m_CurrentIndex].Position);
			m_CurrentIndex++;
		}
	}
}
