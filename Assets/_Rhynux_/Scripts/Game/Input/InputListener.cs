using UniRx;

namespace Rhynux.Game {
	public sealed class InputListener : VContainer.Unity.IStartable, System.IDisposable {
		private readonly InputHandlerFactory m_InputHandler;
		private readonly RefereeFacade m_Referee;

		private readonly CompositeDisposable m_Disposables = new();

		public InputListener (InputHandlerFactory _input, RefereeFacade _referee) {
			m_InputHandler = _input;
			m_Referee = _referee;
		}

		public void Start() {
			m_InputHandler.HandlerPool.OnPressed.Subscribe (x => {
				m_Referee.Press (x);
			}).AddTo (m_Disposables);
		}

		public void Dispose() {
			m_Disposables.Dispose();
		}
	}
}
