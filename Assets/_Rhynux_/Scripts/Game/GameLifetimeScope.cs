using System.Collections;
using System.Linq;
using MackySoft.Navigathena.SceneManagement.VContainer;
using VContainer;
using VContainer.Unity;

public class GameLifetimeScope : LifetimeScope {
	[UnityEngine.SerializeReference] public INotesGenerator m_NotesGenerator;
	[UnityEngine.SerializeField] private ChartAsset m_Chart;

	private _FullLogic m_FullLogic;
	private MusicPlayer m_MusicPlayer;

	private SessionManager m_SessionManager;
	private RealtimeReferee m_RealtimeReferee;
	private ReactiveReferee m_ReactiveReferee;
	private NotesReferee m_NotesReferee;

	protected override void Configure (IContainerBuilder builder) {
		UnityEngine.Debug.Log ("-------- Injection --------");

		builder.RegisterSceneLifecycle<SceneEntryPoint>();

		System.Collections.Generic.List<Note> generatedNotes = m_NotesGenerator.Generate (m_Chart.Chart).ToList();

		SessionData sessionData = new (generatedNotes);

		m_SessionManager = new SessionManager (m_Chart.Chart, generatedNotes);
		m_RealtimeReferee = new RealtimeReferee (generatedNotes);
		m_ReactiveReferee = new ReactiveReferee (generatedNotes);
		m_NotesReferee = new NotesReferee (m_SessionManager, m_RealtimeReferee, m_ReactiveReferee);

		new ComboOperator (m_SessionManager, m_NotesReferee);


		builder.Register (_ => m_Chart.Chart, Lifetime.Singleton);
		builder.Register (_ => sessionData, Lifetime.Singleton);

		builder.RegisterComponentInHierarchy<_FullLogic>();
		builder.RegisterComponentInHierarchy<MusicPlayer>();
		builder.Register (_ => m_SessionManager, Lifetime.Singleton);
		builder.Register (_ => m_RealtimeReferee, Lifetime.Singleton);
		builder.Register (_ => m_ReactiveReferee, Lifetime.Singleton);
		builder.Register (_ => m_NotesReferee, Lifetime.Singleton);

		builder.RegisterComponentInHierarchy<FloorTorquer>();
		builder.RegisterComponentInHierarchy<TrackInfoBanner>();
		builder.RegisterComponentInHierarchy<JudgementDisplay>();
		builder.RegisterComponentInHierarchy<InputVisualizer>();

		builder.RegisterEntryPoint<_FullLogic>();
	}
}
