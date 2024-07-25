using MackySoft.Navigathena.SceneManagement.VContainer;
using VContainer;
using VContainer.Unity;

public class SceneLifetimeScope : VContainer.Unity.LifetimeScope {
	protected override void Configure (IContainerBuilder builder) {
		base.Configure (builder);

		builder.RegisterSceneLifecycle<SceneEntryPoint>();

		builder.RegisterComponentInHierarchy<ScrollItemRegistrar>();
		builder.RegisterComponentInHierarchy<TrackInfoView>();
		builder.RegisterComponentInHierarchy<ScrollView>();
	}
}
