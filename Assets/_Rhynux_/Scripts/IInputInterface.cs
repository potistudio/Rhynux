
public interface IInputInterface {
	System.IObservable<int> OnPressed { get; }
	System.IObservable<int> OnReleased { get; }
}
