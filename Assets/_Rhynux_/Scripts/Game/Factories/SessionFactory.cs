public sealed class SessionFactory {
	private readonly MusicPlayer m_MusicPlayer;

	[VContainer.Inject]
	public SessionFactory (MusicPlayer _musicPlayer) {
		m_MusicPlayer = _musicPlayer;
	}

	// Manual DI
	public SessionData Create (Chart _chart) {
		//* Generate Notes
		var notesGenerator = new ProceduralNotesGenerator(); // flexible
		var notes = notesGenerator.Generate (_chart);

		SessionData session = new (_chart, notes);

		//* Input Handler
		IInputHandler inputHandler = new AutoInputHandler (m_MusicPlayer, session); // flexible

		//* Referee
		ReactiveReferee reactiveReferee = new (session, inputHandler);
		RealtimeReferee realtimeReferee = new (session);
		NotesRefereeComposer notesRefereeComposer = new (session, realtimeReferee, reactiveReferee);

		return session;
	}
}
