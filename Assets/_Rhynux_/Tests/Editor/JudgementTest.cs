
using NUnit.Framework;

public class JudgementTest {
	private RealtimeReferee m_RealtimeReferee = default;

	[SetUp]
	public void SetUp() {
		System.Collections.Generic.List<Note> notes = new() {
			new Note (0f, 0), new Note (1000f, 0), new Note (2000f, 0), new Note (3000f, 0)
		};
		m_RealtimeReferee = new (notes);
	}

	[Test]
	public void Test() {
		m_RealtimeReferee.UpdateTime (2000f);
	}
}
