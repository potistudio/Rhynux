using VContainer;
using VContainer.Unity;

public class GameLifetimeScope : LifetimeScope {
	[UnityEngine.SerializeReference] private IInputInterface m_InputInterface;

    protected override void Configure (IContainerBuilder builder) {
		builder.Register<KeyboardActions>(Lifetime.Singleton);
		builder.Register (m_InputInterface.GetType(), Lifetime.Singleton).As<IInputInterface>();
    }
}
