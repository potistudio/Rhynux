using UnityEngine;

public sealed class Updater : MonoBehaviour {
	private Referee m_Judge;

	[VContainer.Inject]
	private void Inject(Referee referee) {
		m_Judge = referee;
	}

	private void Update() {
		m_Judge.Update(0f);
	}
}
