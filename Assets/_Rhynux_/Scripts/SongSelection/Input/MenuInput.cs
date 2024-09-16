using MackySoft.Navigathena.SceneManagement;

public sealed class MenuInput : UnityEngine.MonoBehaviour {
	private ISceneIdentifier m_SceneIdentifier;
	private Selection m_SelectionAction;

	private SceneNavigator m_SceneNavigator;

	[VContainer.Inject]
	private void Init (SceneNavigator _sceneNavigator) {
		m_SceneNavigator = _sceneNavigator;
	}

	private void Awake() {
		m_SceneIdentifier = new BuiltInSceneIdentifier ("Sample");
		m_SelectionAction = new Selection();

		m_SelectionAction.Menu.MenuNext.performed += NextMenu;
		m_SelectionAction.Menu.MenuBack.performed += BackMenu;

		m_SelectionAction.Enable();
	}

	private void OnEnable() {
		m_SelectionAction?.Enable();
	}

	private void OnDisable() {
		m_SelectionAction?.Disable();
	}

	private void OnDestroy() {
		m_SelectionAction.Dispose();
	}

	private void BackMenu (UnityEngine.InputSystem.InputAction.CallbackContext _context) {
		BackScene();
	}

	private void NextMenu (UnityEngine.InputSystem.InputAction.CallbackContext _context) {
		NextScene();
	}

	private void NextScene() {
		// await GlobalSceneNavigator.Instance.Push (m_SceneIdentifier);
		m_SceneNavigator.StartSession();
	}

	private async void BackScene() {
		await GlobalSceneNavigator.Instance.Pop ();
	}
}
