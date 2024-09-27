public sealed class SessionFactory {
	private SessionData m_SessionPool;
	public SessionData SessionPool => m_SessionPool;

	// Manual DI
	public SessionData Create (Chart _chart) {
		//* Generate Notes
		var notesGenerator = new ProceduralNotesGenerator(); // flexible
		var notes = notesGenerator.Generate (_chart);

		SessionData session = new (_chart, notes);

		UnityEngine.Debug.Log ("Session Generated");

		m_SessionPool = session;
		return session;
	}
}
