using UniRx;

public sealed class JudgementDisplay : VContainer.Unity.IInitializable, System.IDisposable {
	private readonly ReactiveReferee m_ReactiveReferee;
	private readonly AccuracyPopupEmitter m_PopupEmitter;
	private readonly CompositeDisposable m_Disposable = new();

	public JudgementDisplay (ReactiveReferee _reactiveReferee, AccuracyPopupEmitter _accuracyPopupEmitter) {
		m_ReactiveReferee = _reactiveReferee;
		m_PopupEmitter = _accuracyPopupEmitter;
	}

	public void Initialize() {
		m_ReactiveReferee.OnHit.Subscribe (x => {
			m_PopupEmitter.Emit (x.Item3);
		}).AddTo (m_Disposable);
	}

	public void Dispose() {
		m_Disposable.Dispose();
	}
}
