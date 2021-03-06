﻿using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class AnchorToBottom : MonoBehaviour
{
    public float YAnchor = 0.0f;
    private RectTransform m_rect;

    void Start()
    {
        m_rect = GetComponent<RectTransform>();
    }

    void Update()
    {
        LayoutRebuilder.ForceRebuildLayoutImmediate(m_rect);
        m_rect.anchoredPosition = new Vector2(
            m_rect.anchoredPosition.x,
            m_rect.sizeDelta.y/2 + YAnchor
        );
    }
}
