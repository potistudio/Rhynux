public class ScrollerInput : UnityEngine.MonoBehaviour {
	private Selection m_SelectionAction;
	private ScrollView m_ScrollView;

	[VContainer.Inject]
	public void Init (ScrollView _scrollView) {
		m_ScrollView = _scrollView;
		m_SelectionAction = new Selection();

		m_SelectionAction.Track.TrackUp.performed += OnTrackUp;
		m_SelectionAction.Track.TrackDown.performed += OnTrackDown;

		m_SelectionAction.Enable();
	}

	private void OnEnable() {
		m_SelectionAction?.Enable();
	}

	private void OnDisable() {
		m_SelectionAction?.Disable();
	}

	public void OnDestroy() {
		m_SelectionAction.Dispose();
	}

	private void OnTrackUp (UnityEngine.InputSystem.InputAction.CallbackContext _context) {
		m_ScrollView.Next();
	}

	private void OnTrackDown (UnityEngine.InputSystem.InputAction.CallbackContext _context) {
		m_ScrollView.Back();
	}
}
