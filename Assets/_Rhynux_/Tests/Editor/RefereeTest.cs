using NUnit.Framework;
using UniRx;

public class RefereeTest {
	private ReactiveReferee m_ReactiveReferee;
	private RealtimeReferee m_RealtimeReferee;

	[SetUp]
	public void Setup() {
		Note[] notes = {
			new (1.000f, 0),
			new (2.000f, 0),
			new (3.000f, 0)
		};

		SessionData sessionData = new (notes);
		IInputHandler inputHandler = new MockInputHandler();

		// m_ReactiveReferee = new ReactiveReferee (sessionData, inputHandler);
		// m_RealtimeReferee = new RealtimeReferee (sessionData);
	}

	[Test]
	public void CountTest() {
		// First
		m_ReactiveReferee.UpdateTime (1f);
		Assert.That (m_ReactiveReferee.JudgeHit(0), Is.EqualTo(AccuracyLevel.Perfect));

		// Middle
		m_ReactiveReferee.UpdateTime (2f);
		Assert.That (m_ReactiveReferee.JudgeHit(0), Is.EqualTo(AccuracyLevel.Perfect));

		// Last
		m_ReactiveReferee.UpdateTime (3f);
		Assert.That (m_ReactiveReferee.JudgeHit(0), Is.EqualTo(AccuracyLevel.Perfect));
	}

	[Test]
	public void AccuracyTest() {
		// Perfect <= 0.060s
		m_ReactiveReferee.UpdateTime (0.941f);
		Assert.That (m_ReactiveReferee.JudgeHit(0), Is.EqualTo(AccuracyLevel.Perfect));

		m_ReactiveReferee.UpdateTime (0.940f);
		Assert.That (m_ReactiveReferee.JudgeHit(0), Is.EqualTo(AccuracyLevel.Good));

		// Good <= 0.120s
		m_ReactiveReferee.UpdateTime (0.881f);
		Assert.That (m_ReactiveReferee.JudgeHit(0), Is.EqualTo(AccuracyLevel.Good));

		m_ReactiveReferee.UpdateTime (0.880f);
		Assert.That (m_ReactiveReferee.JudgeHit(0), Is.EqualTo(AccuracyLevel.Miss));

		// Miss <= 0.160s
		m_ReactiveReferee.UpdateTime (0.841f);
		Assert.That (m_ReactiveReferee.JudgeHit(0), Is.EqualTo(AccuracyLevel.Miss));

		m_ReactiveReferee.UpdateTime (0.840f);
		Assert.That (m_ReactiveReferee.JudgeHit(0), Is.EqualTo(AccuracyLevel.Pass));
	}

	[Test]
	public void FallTest() {
		(int, NoteAvailableStatus) note = (0, NoteAvailableStatus.Available);

		var o = m_RealtimeReferee.OnNoteStatusChanged.AsObservable();
		o.Subscribe (x => note = x);

		m_RealtimeReferee.UpdateTime (1f);
		Assert.That (note, Is.EqualTo((0, NoteAvailableStatus.Available)));

		m_RealtimeReferee.UpdateTime (2f);
		Assert.That (note, Is.EqualTo((0, NoteAvailableStatus.Fell)));

		m_RealtimeReferee.UpdateTime (3f);
		Assert.That (note, Is.EqualTo((1, NoteAvailableStatus.Fell)));

		m_RealtimeReferee.UpdateTime (4f);
		Assert.That (note, Is.EqualTo((2, NoteAvailableStatus.Fell)));
	}
}
