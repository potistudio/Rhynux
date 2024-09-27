public sealed class RefereeFacade {
	private readonly InputReferee m_InputReferee;
	private readonly RealtimeReferee m_RealtimeReferee;

	private UniRx.Subject<(int index, int lane, AccuracyLevel accuracy)> m_OnHit = new();
	public System.IObservable<(int index, int lane, AccuracyLevel accuracy)> OnHit => m_OnHit;

	public RefereeFacade (SessionFactory _factory) {
		m_InputReferee = new (_factory);
	}

	/*
	public InputReferee m_ReactiveReferee { get; set; }
	public RealtimeReferee m_RealtimeReferee { get; set; }


	// public SessionFacade (ReactiveReferee _referee1, RealtimeReferee _referee2) {
	// 	m_ReactiveReferee = _referee1;
	// 	m_RealtimeReferee = _referee2;
	// }

	public void Start() {
		m_ReactiveReferee.OnHit.Subscribe (x => {
			m_OnHit.OnNext (x);
		});
	}
	*/
}
