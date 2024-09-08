using UnityEngine;
using UniRx;

public class HitEffectGenerator : MonoBehaviour {
	[SerializeField] private GameObject m_HitEffectPrefab;
	[SerializeField] private float m_Lifetime;

	[VContainer.Inject]
	private void Init (ReactiveReferee _reactiveReferee) {
		_reactiveReferee.OnHit.Subscribe (x => {
			if (x.Item3 == AccuracyLevel.Pass)
				return;

			GameObject effect = Instantiate (m_HitEffectPrefab, Vector3.right * ((x.Item2 - 2.5f) * 1.5f), Quaternion.identity);
			Destroy (effect, m_Lifetime);
		});
	}
}
