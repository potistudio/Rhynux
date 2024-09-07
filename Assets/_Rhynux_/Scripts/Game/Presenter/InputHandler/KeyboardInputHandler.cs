[System.Serializable]
public class KeyboardInputHandler : IInputHandler {
	[UnityEngine.SerializeField] private UnityEngine.InputSystem.InputActionAsset m_KeyboardActions;

	private readonly UniRx.Subject<int> m_Pushed = new();
	public System.IObservable<int> OnPressed => m_Pushed;

	private readonly UniRx.Subject<int> m_Released = new();
	public System.IObservable<int> OnReleased => m_Released;

	public KeyboardInputHandler() : base() {
		UnityEngine.Debug.Log ("KeyboardInputHandler");
	}
}
