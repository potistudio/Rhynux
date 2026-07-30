using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Rhynux.SongSelection {
	public sealed class ScrollItemRegistrar : MonoBehaviour {
	    [SerializeField] private bool m_AutoGeneration;
	    [SerializeField] private int m_GenerationCount;

	    // The field was renamed from m_Charts, but the scene still stores the old name,
    // so without this the chart list deserializes as empty.
    [SerializeField, UnityEngine.Serialization.FormerlySerializedAs("m_Charts")]
    private List<ChartAsset> m_ChartAssets = new();
		public List<Chart> Charts => m_ChartAssets.Select(x => x.Unpack()).ToList();

		[VContainer.Inject]
	    private void Init (ScrollView _scrollView) {
			IEnumerable<Chart> generatedCharts;

	        if (m_AutoGeneration) {
	            var i = Enumerable.Range (0, m_GenerationCount);
				// An empty SoundTrack rather than null: consumers reach for Chart.Track.SoundClip,
				// and a null track throws before they get a chance to check the clip.
				generatedCharts = i.Select (_ => new Chart(RandomBase64(), RandomBase64(), 120f, 0f, new SoundTrack(null), new Note[0]));
			} else {
				generatedCharts = m_ChartAssets.Select(x => x.Unpack());
			}

	        _scrollView.UpdateData (generatedCharts.ToArray());
	    }

		private static string RandomBase64() {
			// Generate GUID as a Base64 string
			System.Byte[] guidBytes = System.Guid.NewGuid().ToByteArray();
			string base64 = System.Convert.ToBase64String (guidBytes);

			// Remove last 3 characters '(QAgw)=='.
			// A GUID is always 24 Base64 characters, so slicing beats compiling a Regex per call.
			return base64[..^3] + " ";
		}
	}
}
