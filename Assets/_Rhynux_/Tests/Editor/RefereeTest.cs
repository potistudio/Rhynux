using System.Collections.Generic;
using NUnit.Framework;
using UniRx;

public class RefereeTest {
	private SessionFactory m_SessionFactory;
	private InputReferee m_InputReferee;
	private RealtimeReferee m_RealtimeReferee;

	[SetUp]
	public void Setup() {
		Note[] notes = {
			new (1.000f, 0),
			new (2.000f, 0),
			new (3.000f, 0)
		};

		// 60 BPM makes one beat exactly one second, so the note times above survive
		// the beat-to-seconds conversion in ProceduralNotesGenerator unchanged.
		Chart chart = new ("Test", "Tester", 60f, 0f, new SoundTrack(null), notes);

		m_SessionFactory = new SessionFactory();
		m_SessionFactory.Create (chart);

		m_InputReferee = new InputReferee (m_SessionFactory);
		m_RealtimeReferee = new RealtimeReferee (m_SessionFactory);
	}

	[Test] // Every note in the lane is reachable, including the last one
	public void CountTest() {
		m_InputReferee.UpdateTime (1f);
		Assert.That (m_InputReferee.JudgeHit(0), Is.EqualTo(AccuracyLevel.Perfect));

		m_InputReferee.UpdateTime (2f);
		Assert.That (m_InputReferee.JudgeHit(0), Is.EqualTo(AccuracyLevel.Perfect));

		m_InputReferee.UpdateTime (3f);
		Assert.That (m_InputReferee.JudgeHit(0), Is.EqualTo(AccuracyLevel.Perfect));
	}

	[Test] // Distance from the nearest note maps to the right accuracy band
	public void AccuracyTest() {
		// Values sit clear of the 0.060 / 0.120 / 0.160 boundaries so float error
		// cannot flip the result.

		m_InputReferee.UpdateTime (0.95f);   // 0.05 away
		Assert.That (m_InputReferee.JudgeHit(0), Is.EqualTo(AccuracyLevel.Perfect));

		m_InputReferee.UpdateTime (0.90f);   // 0.10 away
		Assert.That (m_InputReferee.JudgeHit(0), Is.EqualTo(AccuracyLevel.Good));

		m_InputReferee.UpdateTime (0.85f);   // 0.15 away
		Assert.That (m_InputReferee.JudgeHit(0), Is.EqualTo(AccuracyLevel.Miss));

		m_InputReferee.UpdateTime (0.70f);   // 0.30 away
		Assert.That (m_InputReferee.JudgeHit(0), Is.EqualTo(AccuracyLevel.Pass));
	}

	[Test] // The reported index addresses the note that was actually hit
	public void HitReportsSourceIndex() {
		List<int> hits = new();
		m_InputReferee.OnHit.Subscribe (x => hits.Add (x.index));

		m_InputReferee.UpdateTime (3f);
		m_InputReferee.JudgeHit (0);

		Assert.That (hits, Is.EqualTo(new[]{ 2 }));
	}

	[Test] // An empty lane must not report a hit
	public void EmptyLaneNeverHits() {
		List<int> hits = new();
		m_InputReferee.OnHit.Subscribe (x => hits.Add (x.index));

		m_InputReferee.UpdateTime (1f);

		Assert.That (m_InputReferee.JudgeHit(3), Is.EqualTo(AccuracyLevel.Pass));
		Assert.That (hits, Is.Empty);
	}

	[Test] // Notes fall once their judgement window closes
	public void FallTest() {
		(int, NoteAvailableStatus)? last = null;
		m_RealtimeReferee.OnNoteStatusChanged.Subscribe (x => last = x);

		// The margin is 0.160s, so the note at 1.000s survives until 1.160s.
		m_RealtimeReferee.UpdateTime (1.1f);
		Assert.That (last, Is.Null);

		m_RealtimeReferee.UpdateTime (1.2f);
		Assert.That (last, Is.EqualTo((0, NoteAvailableStatus.Fell)));

		m_RealtimeReferee.UpdateTime (2.2f);
		Assert.That (last, Is.EqualTo((1, NoteAvailableStatus.Fell)));

		m_RealtimeReferee.UpdateTime (3.2f);
		Assert.That (last, Is.EqualTo((2, NoteAvailableStatus.Fell)));
	}

	[Test] // Each note falls exactly once, no matter how often time is updated
	public void FallEmitsEachNoteOnce() {
		List<int> fallen = new();
		m_RealtimeReferee.OnNoteStatusChanged.Subscribe (x => fallen.Add (x.Item1));

		m_RealtimeReferee.UpdateTime (5f);
		m_RealtimeReferee.UpdateTime (5f);
		m_RealtimeReferee.UpdateTime (5f);

		Assert.That (fallen, Is.EqualTo(new[]{ 0, 1, 2 }));
	}
}
