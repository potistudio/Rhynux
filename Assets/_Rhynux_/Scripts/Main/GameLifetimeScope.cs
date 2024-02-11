
using VContainer;
using VContainer.Unity;

public class GameLifetimeScope : LifetimeScope {
	[UnityEngine.SerializeReference] private IInputInterface m_InputInterface;
	[UnityEngine.SerializeReference] private INotesGenerator m_NotesGenerator;

    protected override void Configure (IContainerBuilder builder) {
		builder.RegisterComponentInHierarchy<GameStarter>();

		builder.RegisterComponentInHierarchy<NotesObjectGenerator>();

		builder.RegisterComponentInHierarchy<InputReferee>();
		builder.Register (m_NotesGenerator.GetType(), Lifetime.Singleton).As<INotesGenerator>();

		builder.Register<KeyboardActions>(Lifetime.Singleton);
		builder.Register (m_InputInterface.GetType(), Lifetime.Singleton).As<IInputInterface>();
    }
}
