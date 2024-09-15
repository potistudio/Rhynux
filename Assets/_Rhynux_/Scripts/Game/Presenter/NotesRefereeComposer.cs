using System.Linq;
using UniRx;

public sealed class NotesRefereeComposer {
	private readonly RealtimeReferee m_RealtimeReferee;
	private readonly ReactiveReferee m_ReactiveReferee;

	private readonly (Note note, NoteAvailableStatus status)[] m_NotesCollection;

	private readonly Subject<AccuracyLevel> m_OnHit = new();
	public System.IObservable<AccuracyLevel> OnHit => m_OnHit;

	private readonly Subject<Unit> m_OnFall = new();
	public System.IObservable<Unit> OnFall => m_OnFall;

	public NotesRefereeComposer (SessionData _session, RealtimeReferee _realtimeReferee, ReactiveReferee _reactiveReferee) {
		m_RealtimeReferee = _realtimeReferee;
		m_ReactiveReferee = _reactiveReferee;
		m_NotesCollection = _session.Notes.Select (x => (x, NoteAvailableStatus.Available)).ToArray();

		// When Notes Updated by Realtime Referee, also Update the Notes in Session Manager
		m_RealtimeReferee.OnNoteStatusChanged.Subscribe (x => {
			if (m_NotesCollection[x.Item1].Item2 != NoteAvailableStatus.Available)
				return;

			m_NotesCollection[x.Item1].Item2 = NoteAvailableStatus.Fell;
			m_OnHit.OnNext (AccuracyLevel.Miss);
		});

		m_ReactiveReferee.OnHit.Subscribe (x => {
			if (m_NotesCollection[x.index].status != NoteAvailableStatus.Available)
				return;

			m_NotesCollection[x.index].status = NoteAvailableStatus.Hit;
			m_OnHit.OnNext (x.accuracy);
		});
	}
}
