using UniRx;

public sealed class TrackInfoViewPresenter : VContainer.Unity.IInitializable, System.IDisposable {
	private readonly ScrollView m_ScrollView;
	private readonly TrackInfoView m_TrackInfoView;

	private readonly CompositeDisposable m_Disposable = new();

	public TrackInfoViewPresenter (ScrollView _scrollView, TrackInfoView _trackInfoView) {
		m_ScrollView = _scrollView;
		m_TrackInfoView = _trackInfoView;
	}

	public void Initialize() {
		m_ScrollView.OnSelectionChange.Subscribe (x => {
			m_TrackInfoView.ChangeInfoContent (x.Title, x.Artist);
		}).AddTo (m_Disposable);
	}

	public void Dispose() {
		m_Disposable.Dispose();
	}
}
