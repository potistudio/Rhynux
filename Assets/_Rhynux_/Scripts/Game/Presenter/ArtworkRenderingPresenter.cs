public sealed class ArtworkRenderingPresenter : VContainer.Unity.IStartable {
	private readonly SessionProxy m_Session;
	private readonly ArtworkRenderer m_Renderer;

	public ArtworkRenderingPresenter (SessionProxy _session, ArtworkRenderer _renderer) {
		m_Session = _session;
		m_Renderer = _renderer;
	}

	public void Start() {
		Chart chart = m_Session.Session.Chart;
		UnityEngine.Sprite sprite = chart.Artwork;
		m_Renderer.SetSprite (sprite);
	}
}
