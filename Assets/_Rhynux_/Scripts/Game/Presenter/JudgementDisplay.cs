using UniRx;

public sealed class JudgementDisplay : VContainer.Unity.IInitializable, System.IDisposable {
	private readonly NotesRefereeComposer m_NotesReferee;
	private readonly AccuracyPopupEmitter m_PopupEmitter;
	private readonly CompositeDisposable m_Disposable = new();

	public JudgementDisplay (NotesRefereeComposer _referee, AccuracyPopupEmitter _accuracyPopupEmitter) {
		m_NotesReferee = _referee;
		m_PopupEmitter = _accuracyPopupEmitter;
	}

	public void Initialize() {
		m_NotesReferee.OnHit.Subscribe (x => {
			m_PopupEmitter.Emit (x);
		}).AddTo (m_Disposable);
	}

	public void Dispose() {
		m_Disposable.Dispose();
	}
}
