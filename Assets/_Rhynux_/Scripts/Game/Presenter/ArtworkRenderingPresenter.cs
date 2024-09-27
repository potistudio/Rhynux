public sealed class ArtworkRenderingPresenter : VContainer.Unity.IStartable {
	private readonly SessionProxy m_Session;
	private readonly ArtworkRenderer m_Renderer;

	public ArtworkRenderingPresenter (SessionProxy _session, ArtworkRenderer _renderer) {
		m_Session = _session;
		m_Renderer = _renderer;
	}

	public void Start() {
		UnityEngine.Sprite sprite = m_Session.Session.Chart.Artwork;
		m_Renderer.SetSprite (sprite);
	}
}
