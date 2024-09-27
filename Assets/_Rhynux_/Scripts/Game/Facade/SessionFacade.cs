using UniRx;

public sealed class SessionFacade : VContainer.Unity.IStartable {
	public InputReferee m_ReactiveReferee { get; set; }
	public RealtimeReferee m_RealtimeReferee { get; set; }

	private UniRx.Subject<(int index, int lane, AccuracyLevel accuracy)> m_OnHit = new();
	public System.IObservable<(int index, int lane, AccuracyLevel accuracy)> OnHit => m_OnHit;

	// public SessionFacade (ReactiveReferee _referee1, RealtimeReferee _referee2) {
	// 	m_ReactiveReferee = _referee1;
	// 	m_RealtimeReferee = _referee2;
	// }

	public void Start() {
		m_ReactiveReferee.OnHit.Subscribe (x => {
			m_OnHit.OnNext (x);
		});
	}
}
