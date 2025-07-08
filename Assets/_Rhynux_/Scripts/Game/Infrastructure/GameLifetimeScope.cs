using VContainer;
using VContainer.Unity;
using MackySoft.Navigathena.SceneManagement.VContainer;

public sealed class GameLifetimeScope : LifetimeScope {
	[UnityEngine.SerializeField] private ChartAsset m_ChartAsset;

	protected override void Configure (IContainerBuilder builder) {
		Chart unpackedChartInstance = m_ChartAsset.Unpack();

		//* Instance *//
		builder.RegisterInstance<Chart>(unpackedChartInstance);

		//* Lifecycle *//
		builder.RegisterSceneLifecycle<SceneEntryPoint>();
		builder.RegisterComponentInHierarchy<ScopedSceneEntryPoint>();

		//* Factory *//
		// builder.Register<AutoInputHandler>(Lifetime.Singleton);
		// builder.Register<KeyboardInputHandler>(Lifetime.Singleton);

		//* Logic *//
		// builder.Register<InputHandlerFactory>(Lifetime.Singleton);
		// builder.RegisterEntryPoint<InputListener>(Lifetime.Singleton);

		//* Referee
		builder.Register<Referee>(Lifetime.Singleton);
		builder.RegisterComponentInHierarchy<Updater>();
		builder.RegisterComponentInHierarchy<NotesRenderer>();

		// builder.Register<RealtimeReferee>(Lifetime.Singleton);
		// builder.Register<InputReferee>(Lifetime.Singleton);
		// builder.Register<RefereeFacade>(Lifetime.Singleton);
		// builder.RegisterEntryPoint<SessionTimeUpdater>(Lifetime.Singleton);

		//* View *//
		// builder.RegisterEntryPoint<LogicPresenter>(Lifetime.Singleton);
		// builder.RegisterComponentInHierarchy<_FullLogic>();
		// builder.RegisterComponentInHierarchy<FloorTorquer>();
		// builder.RegisterEntryPoint<JudgementDisplay>(Lifetime.Singleton);
		// builder.RegisterComponentInHierarchy<AccuracyPopupEmitter>();
		// builder.RegisterComponentInHierarchy<HitEffectGenerator>();
		// builder.RegisterEntryPoint<HitListener>(Lifetime.Singleton);

		//* Lane Visualizer
		// builder.RegisterEntryPoint<LaneVisualizingPresenter>(Lifetime.Singleton);
		// builder.RegisterComponentInHierarchy<InputVisualizer>();

		//* Combo
		// builder.RegisterEntryPoint<ComboPresenter>(Lifetime.Singleton);
		// builder.RegisterEntryPoint<ComboDisplayPresenter>(Lifetime.Singleton);
		// builder.RegisterComponentInHierarchy<ComboDisplay>();
		// builder.Register<ComboManager>(Lifetime.Singleton);

		//* Score
		// builder.Register<ScoreManager>(Lifetime.Singleton);
		// builder.RegisterComponentInHierarchy<ScoreDisplay>();
		// builder.RegisterEntryPoint<ScorePresenter>(Lifetime.Singleton);
		// builder.RegisterEntryPoint<ScoreDisplayPresenter>(Lifetime.Singleton);

		//* Music Player
		// builder.RegisterEntryPoint<MusicPresenter>(Lifetime.Singleton);
		// builder.RegisterComponentInHierarchy<MusicPlayer>();

		//* Session
		// builder.Register<SessionProxy>(Lifetime.Singleton);
		// builder.Register<SessionFactory>(Lifetime.Singleton);

		//* Track Info View
		// builder.RegisterEntryPoint<TrackInfoPresenter>(Lifetime.Singleton);
		// builder.RegisterComponentInHierarchy<TrackInfoBanner>();

		//* Sprite Renderer
		// builder.RegisterEntryPoint<ArtworkRenderingPresenter>(Lifetime.Singleton);
		// builder.RegisterComponentInHierarchy<ArtworkRenderer>();

		//* Scene Navigation
		// builder.Register<SceneNavigator>(Lifetime.Singleton);
		// builder.RegisterComponentInHierarchy<SceneNavigationInput>();

		UnityEngine.Debug.Log ("[Rhynux] <INFO> - VContainer Injection has Completed");
	}
}
