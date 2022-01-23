using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class SquareUI : MonoBehaviour
{
    public enum Axis
    {
        Horizontal,
        Vertical,
    }

    public Axis drivingAxis;
    private RectTransform rectTransform;

    // Start is called before the first frame update
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.hasChanged)
        {
            if (drivingAxis == Axis.Horizontal)
            {
                rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, rectTransform.sizeDelta.x);
            }
            else
            {
                rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.y, rectTransform.sizeDelta.y);
            }
        }
    }
}
