public class RefereeTest {
	private ReactiveReferee m_ReactiveReferee;

	[NUnit.Framework.SetUp]
	public void Setup() {
		Note[] notes = {
			new (1.000f, 0),
			new (2.000f, 0),
			new (3.000f, 0)
		};

		SessionData sessionData = new (notes);
		IInputHandler inputHandler = new MockInputHandler();
		m_ReactiveReferee = new ReactiveReferee (sessionData, inputHandler);
	}

	[NUnit.Framework.Test]
	public void CountTest() {
		// First
		m_ReactiveReferee.UpdateTime (1f);
		NUnit.Framework.Assert.That (m_ReactiveReferee.JudgeHit(0), NUnit.Framework.Is.EqualTo(AccuracyLevel.Perfect));

		// Middle
		m_ReactiveReferee.UpdateTime (2f);
		NUnit.Framework.Assert.That (m_ReactiveReferee.JudgeHit(0), NUnit.Framework.Is.EqualTo(AccuracyLevel.Perfect));

		// Last
		m_ReactiveReferee.UpdateTime (3f);
		NUnit.Framework.Assert.That (m_ReactiveReferee.JudgeHit(0), NUnit.Framework.Is.EqualTo(AccuracyLevel.Perfect));
	}

	[NUnit.Framework.Test]
	public void AccuracyTest() {
		// Perfect <= 0.060s
		m_ReactiveReferee.UpdateTime (0.941f);
		NUnit.Framework.Assert.That (m_ReactiveReferee.JudgeHit(0), NUnit.Framework.Is.EqualTo(AccuracyLevel.Perfect));

		m_ReactiveReferee.UpdateTime (0.940f);
		NUnit.Framework.Assert.That (m_ReactiveReferee.JudgeHit(0), NUnit.Framework.Is.EqualTo(AccuracyLevel.Good));

		// Good <= 0.120s
		m_ReactiveReferee.UpdateTime (0.881f);
		NUnit.Framework.Assert.That (m_ReactiveReferee.JudgeHit(0), NUnit.Framework.Is.EqualTo(AccuracyLevel.Good));

		m_ReactiveReferee.UpdateTime (0.880f);
		NUnit.Framework.Assert.That (m_ReactiveReferee.JudgeHit(0), NUnit.Framework.Is.EqualTo(AccuracyLevel.Miss));

		// Miss <= 0.160s
		m_ReactiveReferee.UpdateTime (0.841f);
		NUnit.Framework.Assert.That (m_ReactiveReferee.JudgeHit(0), NUnit.Framework.Is.EqualTo(AccuracyLevel.Miss));

		m_ReactiveReferee.UpdateTime (0.840f);
		NUnit.Framework.Assert.That (m_ReactiveReferee.JudgeHit(0), NUnit.Framework.Is.EqualTo(AccuracyLevel.Pass));
	}
}
