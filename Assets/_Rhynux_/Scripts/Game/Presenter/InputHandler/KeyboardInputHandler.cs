[System.Serializable]
public class KeyboardInputHandler : IInputHandler, System.IDisposable {
	private readonly UniRx.Subject<int> m_Pressed = new();
	public System.IObservable<int> OnPressed => m_Pressed;

	private readonly UniRx.Subject<int> m_Released = new();
	public System.IObservable<int> OnReleased => m_Released;

	private KeyboardActions m_KeyboardActions;

	public KeyboardInputHandler() {
		m_KeyboardActions = new();
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

	private void PressedD (UnityEngine.InputSystem.InputAction.CallbackContext _context) { m_Pressed.OnNext (0); }
	private void PressedF (UnityEngine.InputSystem.InputAction.CallbackContext _context) { m_Pressed.OnNext (1); }
	private void PressedJ (UnityEngine.InputSystem.InputAction.CallbackContext _context) { m_Pressed.OnNext (2); }
	private void PressedK (UnityEngine.InputSystem.InputAction.CallbackContext _context) { m_Pressed.OnNext (3); }

	private void ReleasedD (UnityEngine.InputSystem.InputAction.CallbackContext _context) { m_Released.OnNext (0); }
	private void ReleasedF (UnityEngine.InputSystem.InputAction.CallbackContext _context) { m_Released.OnNext (1); }
	private void ReleasedJ (UnityEngine.InputSystem.InputAction.CallbackContext _context) { m_Released.OnNext (2); }
	private void ReleasedK (UnityEngine.InputSystem.InputAction.CallbackContext _context) { m_Released.OnNext (3); }

	public void Dispose() {
		m_KeyboardActions.Pressed.D.performed -= PressedD;
		m_KeyboardActions.Pressed.F.performed -= PressedF;
		m_KeyboardActions.Pressed.J.performed -= PressedJ;
		m_KeyboardActions.Pressed.K.performed -= PressedK;

		m_KeyboardActions.Released.D.performed -= ReleasedD;
		m_KeyboardActions.Released.F.performed -= ReleasedF;
		m_KeyboardActions.Released.J.performed -= ReleasedJ;
		m_KeyboardActions.Released.K.performed -= ReleasedK;

		m_KeyboardActions.Disable();
	}
}
