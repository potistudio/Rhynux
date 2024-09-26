using System;
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
	private NotesRefereeComposer m_NotesReferee;

	protected override void Configure (IContainerBuilder builder) {
		//* Instance *//
		builder.RegisterInstance<Chart>(m_Chart.Chart);

		//* Lifecycle *//
		builder.RegisterSceneLifecycle<SceneEntryPoint>();
		builder.RegisterComponentInHierarchy<ScopedSceneEntryPoint>();

		//* Notes Generator *//
		// System.Collections.Generic.List<Note> generatedNotes = m_NotesGenerator.Generate (m_Chart.Chart).ToList();
		// SessionData sessionData = new (generatedNotes);
		// builder.Register (_ => sessionData, Lifetime.Singleton);
		// builder.Register (_ => m_Chart.Chart, Lifetime.Singleton);

		//* Input Handler *//
		// switch (m_InputHandler) {
		// 	case InputHandlers.Auto:
		// 		builder.RegisterEntryPoint<AutoInputHandler>();
		// 		break;

		// 	case InputHandlers.Keyboard:
		// 		builder.Register<KeyboardInputHandler>(Lifetime.Singleton).As<IInputHandler>();
		// 		break;
		// }

		//* Factory *//
		// builder.Register<InputHandlerFactory>(Lifetime.Singleton);
		// builder.Register<AutoInputHandler>(Lifetime.Singleton);
		// builder.Register<KeyboardInputHandler>(Lifetime.Singleton);

		//* Logic *//
		// builder.Register<RealtimeReferee>(Lifetime.Singleton);
		// builder.Register<ReactiveReferee>(Lifetime.Singleton);
		// builder.Register<NotesRefereeComposer>(Lifetime.Singleton);


		// builder.Register<ScoreManager>(Lifetime.Singleton);
		// builder.Register<ComboManager>(Lifetime.Singleton);

		//* Presenter *//
		// builder.RegisterEntryPoint<SessionTimeUpdater>(Lifetime.Singleton);
		// builder.RegisterEntryPoint<HitListener>(Lifetime.Singleton);
		// builder.RegisterEntryPoint<JudgementDisplay>(Lifetime.Singleton);

		// builder.RegisterEntryPoint<ScorePresenter>(Lifetime.Singleton);
		// builder.RegisterEntryPoint<ScoreDisplayPresenter>(Lifetime.Singleton);

		// builder.RegisterEntryPoint<ComboPresenter>(Lifetime.Singleton);
		// builder.RegisterEntryPoint<ComboDisplayPresenter>(Lifetime.Singleton);


		//* View *//
		// builder.RegisterComponentInHierarchy<_FullLogic>();
		// builder.RegisterComponentInHierarchy<FloorTorquer>();
		// builder.RegisterComponentInHierarchy<InputVisualizer>();
		// builder.RegisterComponentInHierarchy<HitEffectGenerator>();
		// builder.RegisterComponentInHierarchy<AccuracyPopupEmitter>();

		// builder.RegisterComponentInHierarchy<ScoreDisplay>();
		// builder.RegisterComponentInHierarchy<ComboDisplay>();

		//* Music Player
		builder.RegisterEntryPoint<MusicPresenter>(Lifetime.Singleton);
		builder.RegisterComponentInHierarchy<MusicPlayer>();

		//* Session
		builder.Register<SessionProxy>(Lifetime.Singleton);
		builder.Register<SessionFactory>(Lifetime.Singleton);

		//* Track Info View
		builder.RegisterEntryPoint<TrackInfoPresenter>(Lifetime.Singleton);
		builder.RegisterComponentInHierarchy<TrackInfoBanner>();

		//* Scene Navigation
		builder.Register<SceneNavigator>(Lifetime.Singleton);
		builder.RegisterComponentInHierarchy<SceneNavigationInput>();

		UnityEngine.Debug.Log ("VContainer Injection has Completed");
	}

	private enum InputHandlers {
		Auto,
		Keyboard
	}
}
