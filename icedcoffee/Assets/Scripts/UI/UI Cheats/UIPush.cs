using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class UIPush : MonoBehaviour
{
    public RectTransform PushTransform;
    private RectTransform m_transform;

    void Start() {
        m_transform = GetComponent<RectTransform>();
    }

    void Update() {
        // align our bottom with their top
        m_transform.anchoredPosition = new Vector2(
            m_transform.anchoredPosition.x,
            PushTransform.anchoredPosition.y + (PushTransform.sizeDelta.y/2.0f)
        );
        // set our hight to fill the space between our top and bottom anchors
        m_transform.sizeDelta = new Vector2(
            m_transform.sizeDelta.x,
            m_transform.anchorMin.y - m_transform.anchorMax.y - PushTransform.sizeDelta.y
        );
        LayoutRebuilder.ForceRebuildLayoutImmediate(m_transform);
    }
}
