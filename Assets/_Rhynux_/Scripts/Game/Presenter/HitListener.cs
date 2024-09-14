using UniRx;

public class HitListener : System.IDisposable, VContainer.Unity.IInitializable {
	private readonly CompositeDisposable m_Disposables = new();

	private readonly _FullLogic m_FullLogic;
	private readonly ReactiveReferee m_ReactiveReferee;

	public HitListener (_FullLogic _fullLogic, ReactiveReferee _reactiveReferee) {
		m_FullLogic = _fullLogic;
		m_ReactiveReferee = _reactiveReferee;
	}

	public void Initialize() {
		m_ReactiveReferee.OnHit.Subscribe (x => {
			m_FullLogic.DeactivateNote (x.Item1);
		}).AddTo (m_Disposables);
	}

	public void Dispose() {
		m_Disposables.Dispose();
	}
}
