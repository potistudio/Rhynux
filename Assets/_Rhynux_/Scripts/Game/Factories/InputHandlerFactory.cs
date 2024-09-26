using VContainer;
using VContainer.Unity;

public sealed class InputHandlerFactory {
	private readonly IObjectResolver m_Container;

	public InputHandlerFactory (IObjectResolver _container) {
		m_Container = _container;
	}

	public IInputHandler CreateInputHandler (InputMode _mode) {
		switch (_mode) {
			case InputMode.Auto:
				return m_Container.Resolve<AutoInputHandler>();
			case InputMode.Keyboard:
				return m_Container.Resolve<KeyboardInputHandler>();
			case InputMode.Touch:
				throw new System.NotImplementedException();
			default:
				throw new System.ArgumentException ("Unknown InputMode: " + _mode);
		}
	}
}
