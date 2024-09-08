public class MockInputHandler : IInputHandler {
	private readonly UniRx.Subject<int> m_Pressed = new();
	private readonly UniRx.Subject<int> m_Released = new();

	public System.IObservable<int> OnPressed => m_Pressed;
	public System.IObservable<int> OnReleased => m_Released;

	public void Press (int _lane) {
		m_Pressed.OnNext (_lane);
	}

	public void Release (int _lane) {
		m_Released.OnNext (_lane);
	}
}
