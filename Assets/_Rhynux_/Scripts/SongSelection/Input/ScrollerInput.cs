public class ScrollerInput : UnityEngine.MonoBehaviour {
	private Selection m_SelectionAction;
	private ScrollView m_ScrollView;

	[VContainer.Inject]
	public void Init (ScrollView _scrollView) {
		m_ScrollView = _scrollView;
		m_SelectionAction = new Selection();

		m_SelectionAction.Track.TrackUp.performed += OnTrackUp;

		m_SelectionAction.Enable();
	}

	public void OnDestroy() {
		m_SelectionAction.Dispose();
	}

	private void OnTrackUp (UnityEngine.InputSystem.InputAction.CallbackContext _context) {
		UnityEngine.Debug.Log ("Track Up");
		m_ScrollView.Next();
	}
}
