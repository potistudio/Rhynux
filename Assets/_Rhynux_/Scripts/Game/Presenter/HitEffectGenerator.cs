using UnityEngine;
using UniRx;

public sealed class HitEffectGenerator : MonoBehaviour {
	[SerializeField] private GameObject m_PerfectHitEffectPrefab;
	[SerializeField] private GameObject m_GoodHitEffectPrefab;
	[SerializeField] private GameObject m_MissHitEffectPrefab;

	[SerializeField] private float m_Lifetime;

	[VContainer.Inject]
	private void Inject (RefereeFacade _referee) {
		_referee.OnHit.Subscribe (x => {
			GameObject target;

			switch (x.accuracy) {
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

			GameObject effect = Instantiate (target, Vector3.right * ((x.lane - 1.5f) * 1.5f), Quaternion.identity);
			Destroy (effect, m_Lifetime);
		}).AddTo (this);
	}
}
