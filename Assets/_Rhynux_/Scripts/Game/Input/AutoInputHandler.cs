using System.Linq;

public sealed class AutoInputHandler : IInputHandler, VContainer.Unity.ITickable {
	private readonly MusicPlayer m_MusicPlayer;

	private readonly UniRx.Subject<int> m_Pressed = new();
	private readonly UniRx.Subject<int> m_Released = new();

	private readonly System.Collections.ObjectModel.ReadOnlyCollection<Note> m_NotesCollection;
	private int m_CurrentIndex = 0;

	public System.IObservable<int> OnPressed => m_Pressed;
	public System.IObservable<int> OnReleased => m_Released;

	public AutoInputHandler (MusicPlayer _musicPlayer, SessionFactory _session) {
		m_MusicPlayer = _musicPlayer;
		m_NotesCollection = _session.SessionPool.Notes.ToList().AsReadOnly();
	}

	private void Press (int _lane) {
		m_Pressed.OnNext (_lane);
	}

	private async void WaitThenRelease (int _lane) {
		await Cysharp.Threading.Tasks.UniTask.Delay (40);
		Release (_lane);
	}

	private void Release (int _lane) {
		m_Released.OnNext (_lane);
	}

	public void Tick() {
		if (m_CurrentIndex >= m_NotesCollection.Count) {
			UnityEngine.Debug.Log ("End");
			return;
		}

		if (m_MusicPlayer.CurrentTime >= m_NotesCollection[m_CurrentIndex].Time) {
			Press (m_NotesCollection[m_CurrentIndex].Position);
			m_CurrentIndex++;

			WaitThenRelease (m_NotesCollection[m_CurrentIndex - 1].Position);
		}
	}
}
