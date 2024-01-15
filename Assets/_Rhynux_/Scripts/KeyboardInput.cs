
[System.Serializable]
public class KeyboardInput : IInputInterface {
	private KeyboardActions m_KeyboardActions;

	private readonly UniRx.Subject<int> m_OnPressed = new();
	public System.IObservable<int> OnPressed => m_OnPressed;

	private readonly UniRx.Subject<int> m_OnReleased = new();
	public System.IObservable<int> OnReleased => m_OnReleased;

	public KeyboardInput (KeyboardActions _actions) {
		m_KeyboardActions = _actions;
		Enable();
	}

	private void Enable() {
		m_KeyboardActions.Pressed.D.performed += PressedD;
		m_KeyboardActions.Pressed.F.performed += PressedF;
		m_KeyboardActions.Pressed.J.performed += PressedJ;
		m_KeyboardActions.Pressed.K.performed += PressedK;

		m_KeyboardActions.Released.D.performed += ReleasedD;
		m_KeyboardActions.Released.F.performed += ReleasedF;
		m_KeyboardActions.Released.J.performed += ReleasedJ;
		m_KeyboardActions.Released.K.performed += ReleasedK;

		m_KeyboardActions.Enable();
	}

	private void PressedD (UnityEngine.InputSystem.InputAction.CallbackContext _context) { m_OnPressed.OnNext (0); }
	private void PressedF (UnityEngine.InputSystem.InputAction.CallbackContext _context) { m_OnPressed.OnNext (1); }
	private void PressedJ (UnityEngine.InputSystem.InputAction.CallbackContext _context) { m_OnPressed.OnNext (2); }
	private void PressedK (UnityEngine.InputSystem.InputAction.CallbackContext _context) { m_OnPressed.OnNext (3); }

	private void ReleasedD (UnityEngine.InputSystem.InputAction.CallbackContext _context) { m_OnReleased.OnNext (0); }
	private void ReleasedF (UnityEngine.InputSystem.InputAction.CallbackContext _context) { m_OnReleased.OnNext (1); }
	private void ReleasedJ (UnityEngine.InputSystem.InputAction.CallbackContext _context) { m_OnReleased.OnNext (2); }
	private void ReleasedK (UnityEngine.InputSystem.InputAction.CallbackContext _context) { m_OnReleased.OnNext (3); }
}
