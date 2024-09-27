using UniRx;

public sealed class InputListener : VContainer.Unity.IStartable {
	private readonly InputHandlerFactory m_InputHandler;
	private readonly InputReferee m_Referee;

	public InputListener (InputHandlerFactory _input, InputReferee _referee) {
		m_InputHandler = _input;
		m_Referee = _referee;
	}

	public void Start() {
		m_InputHandler.HandlerPool.OnPressed.Subscribe (x => {
			m_Referee.JudgeHit (x);
		});
	}
}
