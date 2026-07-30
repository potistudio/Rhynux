using UnityEngine;
using LitMotion;
using LitMotion.Extensions;

namespace Rhynux.Game {
	public class ComboDisplay : MonoBehaviour {
		[SerializeField] private string m_Format;
	    [SerializeField] private TMPro.TextMeshProUGUI m_Label;

		[Alchemy.Inspector.Title("Animation")]
		[SerializeField] private float m_ScaleMultiplier;
		[SerializeField] private float m_Duration;

		private MotionHandle m_PunchMotion;

		public void SetValue (int _value) {
			m_Label.gameObject.SetActive (_value > 3);

			string formatted = string.Format (m_Format, _value);
			m_Label.text = formatted;

			// The punch has to restart on every combo change, and LitMotion motions are
			// one-shot, so cancel the running one and build a new one.
			m_PunchMotion.TryCancel();

			m_PunchMotion = LMotion.Create (Vector3.one * m_ScaleMultiplier, Vector3.one, m_Duration)
				.WithEase (Ease.OutCubic)
				.BindToLocalScale (m_Label.transform);
		}
	}
}
