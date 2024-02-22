
using NUnit.Framework;
using UnityEngine.ResourceManagement;
using VContainer;

public class GameTest {
	private SessionManager m_SessionManager;
	private RealtimeReferee m_RealtimeReferee;
	private ReactiveReferee m_ReactiveReferee;

	private Chart m_Chart;
	private int m_NotesCount;

	private const string CHART_ASSET_NAME = "人マニア（アーメンの刻みのせ）";

	[SetUp]
	public void SetUp() {
		//* Load Chart Asset Async using Addressable
		m_Chart = UnityEngine.AddressableAssets.Addressables.LoadAssetAsync<Chart>(CHART_ASSET_NAME).WaitForCompletion();

		//* ↓ DI with VContainer ↓ *//
			/*
			// Create Container Builder
			IContainerBuilder containerBuilder = new ContainerBuilder();

			// Register All Classes that Need to be Tested
			containerBuilder.Register<SessionManager>(Lifetime.Singleton).WithParameter(m_Chart);
			containerBuilder.Register<RefereePresenter>(Lifetime.Singleton);
			containerBuilder.Register<RealtimeReferee>(Lifetime.Singleton).WithParameter(m_Chart.Notes as System.Collections.Generic.IList<Note>);

			// Build Container
			using IObjectResolver objectResolver = (containerBuilder as ContainerBuilder).Build();

			// Get Classes and Assign to Member to Test
			m_SessionManager = objectResolver.Resolve<SessionManager>();
			m_RefereePresenter = objectResolver.Resolve<RefereePresenter>();
			m_RealtimeReferee = objectResolver.Resolve<RealtimeReferee>();
			*/
		//* ↑ DI with VContainer ↑ *//

		//* ↓ DI with Manual ↓ *//
		INotesGenerator notesGenerator = new SingleLineConstantIntervalNotesGenerator();
		System.Collections.Generic.List<Note> generatedNotes = notesGenerator.Generate (m_Chart);

		m_SessionManager = new SessionManager (m_Chart, generatedNotes);
		m_RealtimeReferee = new RealtimeReferee (generatedNotes);
		m_ReactiveReferee = new ReactiveReferee (generatedNotes);

		new RefereePresenter (m_SessionManager, m_RealtimeReferee, m_ReactiveReferee);
		new ComboOperator (m_SessionManager, m_ReactiveReferee, m_RealtimeReferee);
		//* ↑ DI with Manual ↑ *//

		m_NotesCount = m_SessionManager.NotesCollection.Count;
	}

	[Test] // RealtimeRefereeが落下処理を正しく行えているかのテスト
	public void RealtimeTest() {
		m_SessionManager.UpdateTime (m_SessionManager.NotesCollection[m_NotesCount - 1].Time + 161f);
		Assert.That (m_SessionManager.NotesCollection[0].AvailableStatus, Is.EqualTo(NoteAvailableStatus.Fell));
		Assert.That (m_SessionManager.NotesCollection[UnityEngine.Mathf.RoundToInt(m_NotesCount / 2)].AvailableStatus, Is.EqualTo(NoteAvailableStatus.Fell));
		Assert.That (m_SessionManager.NotesCollection[m_NotesCount - 1].AvailableStatus, Is.EqualTo(NoteAvailableStatus.Fell));

		m_SessionManager.UpdateTime (0f);
		Assert.That (m_SessionManager.NotesCollection[0].AvailableStatus, Is.EqualTo(NoteAvailableStatus.Available));
		Assert.That (m_SessionManager.NotesCollection[UnityEngine.Mathf.RoundToInt(m_NotesCount / 2)].AvailableStatus, Is.EqualTo(NoteAvailableStatus.Available));
		Assert.That (m_SessionManager.NotesCollection[m_NotesCount - 1].AvailableStatus, Is.EqualTo(NoteAvailableStatus.Available));
	}

	[Test] // 時間によって正しく判定が行われるかのテスト
	public void ReactiveTest() {
		m_SessionManager.UpdateTime (m_SessionManager.NotesCollection[0].Time);
		Assert.That (m_ReactiveReferee.JudgeHit(0), Is.EqualTo(AccuracyLevel.Perfect));
		m_SessionManager.UpdateTime (m_SessionManager.NotesCollection[1].Time + 60f);
		Assert.That (m_ReactiveReferee.JudgeHit(0), Is.EqualTo(AccuracyLevel.Perfect));

		m_SessionManager.UpdateTime (m_SessionManager.NotesCollection[2].Time + 61f);
		Assert.That (m_ReactiveReferee.JudgeHit(0), Is.EqualTo(AccuracyLevel.Good));
		m_SessionManager.UpdateTime (m_SessionManager.NotesCollection[3].Time + 120f);
		Assert.That (m_ReactiveReferee.JudgeHit(0), Is.EqualTo(AccuracyLevel.Good));

		m_SessionManager.UpdateTime (m_SessionManager.NotesCollection[4].Time + 121f);
		Assert.That (m_ReactiveReferee.JudgeHit(0), Is.EqualTo(AccuracyLevel.Miss));

		m_SessionManager.UpdateTime (m_SessionManager.NotesCollection[^1].Time + 161f);
		Assert.That (m_ReactiveReferee.JudgeHit(0), Is.EqualTo(AccuracyLevel.Pass));
	}

	[Test] // 一度ヒットしたノーツがRealtimeRefereeによって上書きされないかのテスト
	public void MutualInterferenceTest() {
		m_SessionManager.UpdateTime (m_SessionManager.NotesCollection[0].Time);
		m_ReactiveReferee.JudgeHit(0);

		Assert.That (m_SessionManager.NotesCollection[0].AvailableStatus, Is.EqualTo(NoteAvailableStatus.Hit));

		m_SessionManager.UpdateTime (m_SessionManager.NotesCollection[20].Time);
		Assert.That (m_SessionManager.NotesCollection[0].AvailableStatus, Is.EqualTo(NoteAvailableStatus.Hit));

		m_SessionManager.UpdateTime (m_SessionManager.NotesCollection[0].Time);
		Assert.That (m_SessionManager.NotesCollection[0].AvailableStatus, Is.EqualTo(NoteAvailableStatus.Available));

		m_SessionManager.UpdateTime (m_SessionManager.NotesCollection[20].Time);
		Assert.That (m_SessionManager.NotesCollection[0].AvailableStatus, Is.EqualTo(NoteAvailableStatus.Fell));
	}

	[Test] // コンボが正しく操作されているかテスト
	public void ComboOperationTest() {
		m_SessionManager.UpdateTime (m_SessionManager.NotesCollection[0].Time);
		m_ReactiveReferee.JudgeHit(0);
		Assert.That (m_SessionManager.CurrentCombo, Is.EqualTo(1));

		m_SessionManager.UpdateTime (m_SessionManager.NotesCollection[3].Time); // Fall
		Assert.That (m_SessionManager.CurrentCombo, Is.EqualTo(0));

		m_SessionManager.UpdateTime (m_SessionManager.NotesCollection[3].Time + 120f); // Good
		m_ReactiveReferee.JudgeHit(0);
		Assert.That (m_SessionManager.CurrentCombo, Is.EqualTo(1));

		m_SessionManager.UpdateTime (m_SessionManager.NotesCollection[4].Time + 121f); // Miss
		m_ReactiveReferee.JudgeHit(0);
		Assert.That (m_SessionManager.CurrentCombo, Is.EqualTo(0));
	}

	[Test, Unity.PerformanceTesting.Performance]
	public void TimeChangingPerformance() {
		Unity.PerformanceTesting.Measure.Method (() => {
			m_SessionManager.UpdateTime (m_SessionManager.NotesCollection[m_NotesCount - 1].Time + 161f);
			m_SessionManager.UpdateTime (0f);
		})
		.WarmupCount (16)
		.IterationsPerMeasurement (1000)
		.MeasurementCount (16)
		.Run();
	}
}
