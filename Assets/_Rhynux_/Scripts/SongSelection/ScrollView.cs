
using UnityEngine;
using FancyScrollView;
using VContainer;
using System.Linq;


public class ScrollView : FancyScrollView<Chart> {
    [SerializeField] private Scroller m_Scroller;
    [SerializeField] private TrackInfoView m_SongInfoView;
    [SerializeField] private SongPreviewSource m_SongPreviewSource;
    [SerializeField] private GameObject m_CellPrefab;

    [SerializeField] private Transform m_TurnTable;

    private float m_ViewStartAngle = 0f;
    private float m_ViewEndAngle = 0f;

    private int m_LastSelectedIndex = -1;

    protected override GameObject CellPrefab => m_CellPrefab;

    private Chart[] m_Charts;

    [Inject]
    private void Init (TrackInfoView _songInfoView) {
        m_Scroller.OnValueChanged (OnValueChanged);
        m_Scroller.OnSelectionChanged (OnSelectionChanged);

        m_ViewStartAngle = m_CellPrefab.GetComponent<ScrollCell>().StartAngle;
        m_ViewEndAngle = m_CellPrefab.GetComponent<ScrollCell>().EndAngle;

        m_SongInfoView = _songInfoView;
    }

    public void UpdateData (System.Collections.Generic.IList<Chart> data) {
        base.UpdateContents (data);
        m_Charts = data.ToArray();

        m_Scroller.SetTotalCount (data.Count);

        OnSelectionChanged (0);
    }

    private void OnValueChanged (float _position) {
        base.UpdatePosition(_position);

        float deltaAngleRad = (m_ViewEndAngle - m_ViewStartAngle) / (1f / 0.16f) * Mathf.Deg2Rad;
        m_TurnTable.localEulerAngles = Vector3.back * (deltaAngleRad * _position * Mathf.Rad2Deg);
    }

    private void OnSelectionChanged (int _index) {
        if (_index == m_LastSelectedIndex)
            return;

        // m_SongPreviewSource.ChangePreviewClip (m_Charts[_index].PreviewClip);
        m_SongInfoView.ChangeInfoContent (m_Charts[_index].Title, m_Charts[_index].Artist);
        // m_SongInfoView.ChangeCoverImage (m_Charts[_index].Cover);

        m_LastSelectedIndex = _index;
    }
}
