using UniRx;

public sealed class HitListener : System.IDisposable, VContainer.Unity.IInitializable {
	private readonly CompositeDisposable m_Disposables = new();

	private readonly _FullLogic m_FullLogic;
	private readonly RefereeFacade m_Referee;

	public HitListener (_FullLogic _fullLogic, RefereeFacade _referee) {
		m_FullLogic = _fullLogic;
		m_Referee = _referee;
	}

	public void Initialize() {
		m_Referee.OnHit.Subscribe (x => {
			m_FullLogic.DeactivateNote (x.Item1);
		}).AddTo (m_Disposables);
	}

	public void Dispose() {
		m_Disposables.Dispose();
	}
}
