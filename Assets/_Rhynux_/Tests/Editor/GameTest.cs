using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

using Rhynux.Game;

namespace Rhynux.Tests {
	public class GameTest {
		private Chart m_Chart;

		[SetUp]
		public void SetUp() {
			Note[] notes = {
				new (0f, 0),
				new (1f, 1),
				new (2f, 2),
				new (3f, 3)
			};

			// Built in-process rather than loaded through Addressables: Chart is a plain
			// C# class, not a UnityEngine.Object, so it was never loadable that way. The
			// chart assets also live outside the repository.
			m_Chart = new ("Test", "Tester", 60f, 0f, new SoundTrack(null), notes);
		}

		[Test] // Combo has to survive between calls
		public void ComboAccumulates() {
			SessionManager session = new (m_Chart);

			Assert.That (session.CurrentCombo.Value, Is.EqualTo(0));

			session.IncreaseCombo();
			session.IncreaseCombo();
			session.IncreaseCombo();
			Assert.That (session.CurrentCombo.Value, Is.EqualTo(3));

			session.ResetCombo();
			Assert.That (session.CurrentCombo.Value, Is.EqualTo(0));
		}

		[Test] // Reading the exposed view repeatedly must not detach it from the source
		public void ComboViewIsStable() {
			SessionManager session = new (m_Chart);

			UniRx.ReadOnlyReactiveProperty<int> first = session.CurrentCombo;
			session.IncreaseCombo();
			UniRx.ReadOnlyReactiveProperty<int> second = session.CurrentCombo;

			Assert.That (first, Is.SameAs(second));
			Assert.That (first.Value, Is.EqualTo(1));
		}

		[Test]
		public void ScoreAccumulates() {
			SessionManager session = new (m_Chart);

			session.AddScore (120);
			session.AddScore (80);

			Assert.That (session.CurrentScore.Value, Is.EqualTo(200));
		}

		[Test] // Beats convert by tempo; the offset is already in seconds
		public void GeneratorAppliesOffsetInSeconds() {
			Chart chart = new ("Offset", "Tester", 120f, 0.5f, new SoundTrack(null), new Note[]{ new (2f, 0) });

			IList<Note> generated = new ProceduralNotesGenerator().Generate (chart);

			// 2 beats at 120 BPM is 1.0s, plus the 0.5s offset.
			Assert.That (generated.Single().Time, Is.EqualTo(1.5f).Within(0.0001f));
		}

		[Test]
		public void SessionExposesEveryNote() {
			SessionFactory factory = new();
			SessionData session = factory.Create (m_Chart);

			Assert.That (session.Notes.Count, Is.EqualTo(4));
			Assert.That (session.Notes.Select (x => x.Position), Is.EqualTo(new[]{ 0, 1, 2, 3 }));
		}

		[Test] // The same collection instance is handed out, not a fresh copy per read
		public void SessionNotesDoNotReallocate() {
			SessionFactory factory = new();
			SessionData session = factory.Create (m_Chart);

			Assert.That (session.Notes, Is.SameAs(session.Notes));
		}
	}
}
