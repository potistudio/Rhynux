
using System.Linq;
using UniRx;

public class NotesObjectObserver : UnityEngine.MonoBehaviour {
	[UnityEngine.SerializeField] private NotesObjectGenerator m_NotesObjectGenerator;
	[UnityEngine.SerializeField] private float m_NotesScrollSpeed;

	private SessionManager m_SessionManager;
	private float m_CurrentTime;

	private System.Collections.Generic.List<(Note, UnityEngine.GameObject)> m_NotesList = new();

	[VContainer.Inject]
	private void Init (SessionManager _sessionManager) {
		m_SessionManager = _sessionManager;
	}

	private void Start() {
		var a = m_SessionManager.NotesCollection;
		var o = m_NotesObjectGenerator.GenerateNotes (a.Select(x => { var y = x.DeepCopy(); y.Time /= 1000f; return y; }).ToList());

		for (int i = 0; i < o.Count; i++)
			m_NotesList.Add ((a[i], o[i]));

		m_SessionManager.OnTimeUpdated.Subscribe (x => {
			MoveNotes (x);
		}).AddTo (this);
	}

	private void MoveNotes (float _targetTime) {
		foreach (var note in m_NotesList) {
			UnityEngine.Transform noteTransform = note.Item2.transform;
			noteTransform.position = new UnityEngine.Vector3 (noteTransform.position.x, noteTransform.position.y, (note.Item1.Time / 1000f * m_NotesScrollSpeed) - (_targetTime / 1000f * m_NotesScrollSpeed));
		}

		m_CurrentTime = _targetTime;
	}
}
