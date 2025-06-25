using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public sealed class ScrollItemRegistrar : MonoBehaviour {
    [SerializeField] private bool m_AutoGeneration;
    [SerializeField] private int m_GenerationCount;

    [SerializeField] private List<ChartAsset> m_ChartAssets = new();
	public List<Chart> Charts => m_ChartAssets.Select(x => x.Unpack()).ToList();

	[VContainer.Inject]
    private void Init (ScrollView _scrollView) {
		IEnumerable<Chart> generatedCharts;

        if (m_AutoGeneration) {
            var i = Enumerable.Range (0, m_GenerationCount);
			generatedCharts = i.Select (_ => new Chart(RandomBase64(), RandomBase64(), 120f, 0f, null, new Note[0]));
		} else {
			generatedCharts = m_ChartAssets.Select(x => x.Unpack());
		}

        _scrollView.UpdateData (generatedCharts.ToArray());
    }

	private string RandomBase64() {
		// Generate GUID as a Base64 string
		System.Byte[] guidBytes = System.Guid.NewGuid().ToByteArray();
		string base64 = System.Convert.ToBase64String (guidBytes);

		// Remove last 3 characters '(QAgw)=='
		System.Text.RegularExpressions.Regex regex = new (".{3}$");
		string result = regex.Replace (base64, " ");

		return result;
	}
}
