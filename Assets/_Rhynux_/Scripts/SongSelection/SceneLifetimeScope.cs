using MackySoft.Navigathena.SceneManagement.VContainer;
using VContainer;
using VContainer.Unity;

public class SceneLifetimeScope : VContainer.Unity.LifetimeScope {
	protected override void Configure (IContainerBuilder builder) {
		base.Configure (builder);

		builder.RegisterSceneLifecycle<SceneEntryPoint>();
		builder.Register<SceneNavigator>(Lifetime.Singleton);

		builder.RegisterComponentInHierarchy<MenuInput>();
		builder.RegisterEntryPoint<TrackInfoViewPresenter>(Lifetime.Singleton);

		builder.RegisterComponentInHierarchy<ScrollItemRegistrar>();
		builder.RegisterComponentInHierarchy<TrackInfoView>();
		builder.RegisterComponentInHierarchy<ScrollView>();
		builder.RegisterComponentInHierarchy<ScrollerInput>();
		builder.RegisterComponentInHierarchy<AudioEmitter>();
	}
}
