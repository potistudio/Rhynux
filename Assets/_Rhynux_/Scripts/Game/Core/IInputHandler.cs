public interface IInputHandler {
	System.IObservable<int> OnPressed { get; }
	System.IObservable<int> OnReleased { get; }
}
