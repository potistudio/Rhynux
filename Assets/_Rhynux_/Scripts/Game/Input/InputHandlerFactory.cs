using VContainer;
using VContainer.Unity;

public sealed class InputHandlerFactory {
	private readonly IObjectResolver m_Container;

	private IInputHandler m_HandlerPool;
	public IInputHandler HandlerPool => m_HandlerPool;

	public InputHandlerFactory (IObjectResolver _container) {
		m_Container = _container;
	}

	public IInputHandler Create (InputMode _mode) {
		IInputHandler handler;

		switch (_mode) {
			case InputMode.Auto:
				handler = m_Container.Resolve<AutoInputHandler>();
				break;
			case InputMode.Keyboard:
				handler = m_Container.Resolve<KeyboardInputHandler>();
				break;
			case InputMode.Touch:
				throw new System.NotImplementedException();
			default:
				throw new System.ArgumentException ("Unknown InputMode: " + _mode);
		}

		UnityEngine.Debug.Log ("Input Handler Created: " + _mode);

		m_HandlerPool = handler;
		return handler;
	}
}
