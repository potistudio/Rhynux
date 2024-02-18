
using NUnit.Framework;
using UnityEngine.ResourceManagement;
using VContainer;

public class GameTest {
	private SessionManager m_SessionManager;
	private RealtimeReferee m_RealtimeReferee;

	private Chart m_Chart;

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

		new RefereePresenter (m_SessionManager, m_RealtimeReferee);
		//* ↑ DI with Manual ↑ *//
	}

	[Test]
	public void Test() {
		int notesCount = m_SessionManager.NotesCollection.Count;

		m_SessionManager.UpdateTime (m_SessionManager.NotesCollection[notesCount - 1].Time + 161f);
		Assert.That (m_SessionManager.NotesCollection[0].AvailableStatus, Is.EqualTo(NoteAvailableStatus.Fell));
		Assert.That (m_SessionManager.NotesCollection[UnityEngine.Mathf.RoundToInt(notesCount / 2)].AvailableStatus, Is.EqualTo(NoteAvailableStatus.Fell));
		Assert.That (m_SessionManager.NotesCollection[notesCount - 1].AvailableStatus, Is.EqualTo(NoteAvailableStatus.Fell));

		m_SessionManager.UpdateTime (0f);
		Assert.That (m_SessionManager.NotesCollection[0].AvailableStatus, Is.EqualTo(NoteAvailableStatus.Available));
		Assert.That (m_SessionManager.NotesCollection[UnityEngine.Mathf.RoundToInt(notesCount / 2)].AvailableStatus, Is.EqualTo(NoteAvailableStatus.Available));
		Assert.That (m_SessionManager.NotesCollection[notesCount - 1].AvailableStatus, Is.EqualTo(NoteAvailableStatus.Available));
	}
}
