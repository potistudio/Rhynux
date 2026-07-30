using System.Threading.Tasks;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Rhynux.Game {
	public class _FullLogic : MonoBehaviour {
		[SerializeField] private GameObject m_NotePrefab;
		[SerializeField] private float m_ScrollSpeed;
		[SerializeField] private float m_UserOffset;
		[SerializeField] private float m_FloorWidth;
		[SerializeField] private Transform m_NotesContainer;

		private MusicPlayer m_MusicPlayer;
		private SessionProxy m_Session;

		private System.Collections.Generic.List<(Note, GameObject)> m_NoteObjects = new();

		[VContainer.Inject]
		private void Inject (MusicPlayer _musicPlayer, SessionProxy _session) {
			m_MusicPlayer = _musicPlayer;
			m_Session = _session;
		}

		public void Init() {
			foreach (Note note in m_Session.Session.Notes) {
				GameObject no = Instantiate(m_NotePrefab, Vector3.zero, Quaternion.identity);
				m_NoteObjects.Add ((note, no));
				no.transform.SetParent (m_NotesContainer);
			}
		}

		public void DeactivateNote (int _index) {
			m_NoteObjects[_index].Item2.SetActive (false);
		}

		private void Update() {
			foreach (var x in m_NoteObjects) {
				x.Item2.transform.localPosition = new Vector3 (
					(x.Item1.Position - 1.5f) * 1.5f,
					0f,
					((x.Item1.Time + m_UserOffset) + (-m_MusicPlayer.CurrentTime)) * m_ScrollSpeed
				);
			}
		}

	}
}
