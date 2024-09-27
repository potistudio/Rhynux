using UniRx;

public sealed class JudgementDisplay : VContainer.Unity.IInitializable, System.IDisposable {
	private readonly RefereeFacade m_Referee;
	private readonly AccuracyPopupEmitter m_PopupEmitter;
	private readonly CompositeDisposable m_Disposable = new();

	public JudgementDisplay (RefereeFacade _referee, AccuracyPopupEmitter _accuracyPopupEmitter) {
		m_Referee = _referee;
		m_PopupEmitter = _accuracyPopupEmitter;
	}

	public void Initialize() {
		m_Referee.OnHit.Subscribe (x => {
			m_PopupEmitter.Emit (x.accuracy);
		}).AddTo (m_Disposable);
	}

	public void Dispose() {
		m_Disposable.Dispose();
	}
}
