
[System.Serializable]
public class KeyboardInput : IInputInterface {
	private KeyboardActions m_KeyboardActions;

	private readonly UniRx.Subject<int> m_OnPressed = new();
	public System.IObservable<int> OnPressed => m_OnPressed;

	private readonly UniRx.Subject<int> m_OnReleased = new();
	public System.IObservable<int> OnReleased => m_OnReleased;

	public KeyboardInput() {
		m_KeyboardActions = new KeyboardActions();
		Enable();
	}

	private void Enable() {
		m_KeyboardActions.Pressed.F.performed += PressedF;

		m_KeyboardActions.Enable();
	}

	private void PressedF (UnityEngine.InputSystem.InputAction.CallbackContext _context) { m_OnPressed.OnNext (1); }
}
