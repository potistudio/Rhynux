using System.Collections;
using System.Linq;
using MackySoft.Navigathena.SceneManagement.VContainer;
using VContainer;
using VContainer.Unity;

public class GameLifetimeScope : LifetimeScope {
	[UnityEngine.SerializeReference] public INotesGenerator m_NotesGenerator;
	// [UnityEngine.SerializeReference] public IInputHandler m_InputHandler;
	[UnityEngine.SerializeField] private InputHandlers m_InputHandler;
	[UnityEngine.SerializeField] private ChartAsset m_Chart;

	private _FullLogic m_FullLogic;
	private MusicPlayer m_MusicPlayer;

	private SessionManager m_SessionManager;
	private RealtimeReferee m_RealtimeReferee;
	private ReactiveReferee m_ReactiveReferee;
	private NotesReferee m_NotesReferee;

	protected override void Configure (IContainerBuilder builder) {
		UnityEngine.Debug.Log ("-------- Injection --------");

		//* Lifecycle *//
		builder.RegisterSceneLifecycle<SceneEntryPoint>();

		//* Notes Generator *//
		System.Collections.Generic.List<Note> generatedNotes = m_NotesGenerator.Generate (m_Chart.Chart).ToList();
		SessionData sessionData = new (generatedNotes);
		builder.Register (_ => sessionData, Lifetime.Singleton);
		builder.Register (_ => m_Chart.Chart, Lifetime.Singleton);

		//* Input Handler *//
		switch (m_InputHandler) {
			case InputHandlers.Auto:
				builder.RegisterEntryPoint<AutoInputHandler>();
				break;

			case InputHandlers.Keyboard:
				builder.Register<KeyboardInputHandler>(Lifetime.Singleton).As<IInputHandler>();
				break;
		}

		//* Logic *//
		builder.Register<ReactiveReferee>(Lifetime.Singleton);

		//* Presenter *//
		builder.RegisterEntryPoint<SessionTimeUpdater>(Lifetime.Singleton);
		builder.RegisterEntryPoint<HitListener>(Lifetime.Singleton);
		builder.RegisterEntryPoint<JudgementDisplay>(Lifetime.Singleton);

		//* View *//
		builder.RegisterComponentInHierarchy<_FullLogic>();
		builder.RegisterComponentInHierarchy<MusicPlayer>();
		builder.RegisterComponentInHierarchy<FloorTorquer>();
		builder.RegisterComponentInHierarchy<TrackInfoBanner>();
		builder.RegisterComponentInHierarchy<InputVisualizer>();
		builder.RegisterComponentInHierarchy<HitEffectGenerator>();
		builder.RegisterComponentInHierarchy<AccuracyPopupEmitter>();
	}

	private enum InputHandlers {
		Auto,
		Keyboard
	}
}
