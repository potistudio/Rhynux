
using System.Linq;
using UniRx;

public class NotesObjectObserver : UnityEngine.MonoBehaviour {
	[UnityEngine.SerializeField] private JudgementDisplay m_JudgementDisplay;
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
		System.Collections.ObjectModel.ReadOnlyCollection<Note> a = m_SessionManager.NotesCollection;
		System.Collections.Generic.List<UnityEngine.GameObject> o = m_NotesObjectGenerator.GenerateNotes (a.Select(x => { var y = x.DeepCopy(); y.Time /= 1000f; return y; }).ToList());

		for (int i = 0; i < o.Count; i++)
			m_NotesList.Add ((a[i], o[i]));

		m_SessionManager.OnTimeUpdated.Subscribe (x => {
			MoveNotes (x);
		}).AddTo (this);

		m_SessionManager.OnNoteDisabled.Subscribe (x => {
			int index = m_NotesList.FindIndex (y => y.Item1 == x);
			m_NotesList[index].Item2.SetActive (false);

			if (x.AvailableStatus == NoteAvailableStatus.Fell)
				m_JudgementDisplay.ShowPopup ("Miss");
		}).AddTo (this);
	}

	private void MoveNotes (float _targetTime) {
		foreach ((Note, UnityEngine.GameObject) note in m_NotesList) {
			UnityEngine.Transform noteTransform = note.Item2.transform;
			noteTransform.position = new UnityEngine.Vector3 (noteTransform.position.x, noteTransform.position.y, (note.Item1.Time / 1000f * m_NotesScrollSpeed) - (_targetTime / 1000f * m_NotesScrollSpeed));
		}

		m_CurrentTime = _targetTime;
	}
}
