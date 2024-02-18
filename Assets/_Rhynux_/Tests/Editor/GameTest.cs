
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
		m_SessionManager = new SessionManager (m_Chart);
		m_RealtimeReferee = new RealtimeReferee (m_Chart.Notes);

		//TODO: Insert Notes Generator

		new RefereePresenter (m_SessionManager, m_RealtimeReferee);
		//* ↑ DI with Manual ↑ *//
	}

	[Test]
	public void Test() {
		m_SessionManager.UpdateTime (1000f);
		Assert.That (m_SessionManager.NotesCollection[0].AvailableStatus, Is.EqualTo(NoteAvailableStatus.Fell));
	}
}
