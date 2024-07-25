
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class ScrollItemRegistrar : MonoBehaviour {
    [SerializeField] private bool m_AutoGeneration;
    [SerializeField] private int m_GenerationCount;

    [SerializeField] private List<ChartAsset> m_Charts = new();

	[VContainer.Inject]
    private void Init (ScrollView _scrollView) {
        if (m_AutoGeneration)
            Enumerable.Range (0, m_GenerationCount);

        _scrollView.UpdateData (m_Charts.Select(x => x.Chart).ToArray());
    }
}
