public sealed class SessionFactory {
	private readonly MusicPlayer m_MusicPlayer;
	private readonly SessionFacade m_Facade;

	[VContainer.Inject]
	public SessionFactory (MusicPlayer _musicPlayer, SessionFacade _facade) {
		m_MusicPlayer = _musicPlayer;
		m_Facade = _facade;
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

		m_Facade.m_ReactiveReferee = reactiveReferee;
		m_Facade.m_RealtimeReferee = realtimeReferee;

		return session;
	}
}
