using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollViewContentAutoAdjustHeight : MonoBehaviour
{
    [SerializeField]
    private RectTransform rectTransform = null;

    [SerializeField]
    private float cellHeight = 100f;
    private Vector2 size;
    private Vector2 oldSize;
    private Vector2 deltaSize;

    private void Update()
    {
        size = new Vector2(rectTransform.rect.width, cellHeight * transform.childCount);
        oldSize = rectTransform.rect.size;
        deltaSize = size - oldSize;
        rectTransform.offsetMin = rectTransform.offsetMin - new Vector2(deltaSize.x * rectTransform.pivot.x, deltaSize.y * rectTransform.pivot.y);
        rectTransform.offsetMax = rectTransform.offsetMax + new Vector2(deltaSize.x * (1f - rectTransform.pivot.x), deltaSize.y * (1f - rectTransform.pivot.y));
    }
}
