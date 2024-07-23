
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class ScrollItemRegistrar : MonoBehaviour {
    [SerializeField] private bool m_AutoGeneration;
    [SerializeField] private int m_GenerationCount;

    [SerializeField] private List<Chart> m_Charts = new();

    [SerializeField] private ScrollView m_ScrollView;

    private void Start() {
        if (m_AutoGeneration) {
            Enumerable.Range (0, m_GenerationCount);
        }

        m_ScrollView.UpdateData (m_Charts);
    }
}
