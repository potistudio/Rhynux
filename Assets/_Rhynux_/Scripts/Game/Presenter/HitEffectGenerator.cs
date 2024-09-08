using UnityEngine;
using UniRx;

public class HitEffectGenerator : MonoBehaviour {
	[SerializeField] private GameObject m_PerfectHitEffectPrefab;
	[SerializeField] private GameObject m_GoodHitEffectPrefab;
	[SerializeField] private GameObject m_MissHitEffectPrefab;

	[SerializeField] private float m_Lifetime;

	[VContainer.Inject]
	private void Init (ReactiveReferee _reactiveReferee) {
		GameObject target;

		_reactiveReferee.OnHit.Subscribe (x => {
			switch (x.Item3) {
				case AccuracyLevel.Perfect:
					target = m_PerfectHitEffectPrefab;
					break;

				case AccuracyLevel.Good:
					target = m_GoodHitEffectPrefab;
					break;

				case AccuracyLevel.Miss:
					target = m_MissHitEffectPrefab;
					break;

				default:
					return;
			}

			GameObject effect = Instantiate (target, Vector3.right * ((x.Item2 - 2.5f) * 1.5f), Quaternion.identity);
			Destroy (effect, m_Lifetime);
		});
	}
}
