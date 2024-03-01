
using VContainer;
using VContainer.Unity;

public class GameLifetimeScope : LifetimeScope {
	[UnityEngine.SerializeReference] private INotesGenerator m_NotesGenerator;
	[UnityEngine.SerializeField] private Chart m_Chart;

	private SessionManager m_SessionManager;
	private RealtimeReferee m_RealtimeReferee;
	private ReactiveReferee m_ReactiveReferee;
	private NotesReferee m_NotesReferee;

    protected override void Configure (IContainerBuilder builder) {
		INotesGenerator notesGenerator = new SingleLineConstantIntervalNotesGenerator();
		System.Collections.Generic.List<Note> generatedNotes = notesGenerator.Generate (m_Chart);

		m_SessionManager = new SessionManager (m_Chart, generatedNotes);
		m_RealtimeReferee = new RealtimeReferee (generatedNotes);
		m_ReactiveReferee = new ReactiveReferee (generatedNotes);
		m_NotesReferee = new NotesReferee (m_SessionManager, m_RealtimeReferee, m_ReactiveReferee);

		new ComboOperator (m_SessionManager, m_NotesReferee);

		builder.Register (_ => m_SessionManager, Lifetime.Singleton);
		builder.Register (_ => m_RealtimeReferee, Lifetime.Singleton);
		builder.Register (_ => m_ReactiveReferee, Lifetime.Singleton);
		builder.Register (_ => m_NotesReferee, Lifetime.Singleton);
    }
}
