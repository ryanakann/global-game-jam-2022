using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropManager : Singleton<DropManager>
{

    RectTransform waxDropZone, peanutDropZone;
    [HideInInspector]
    public Camera cam;

    [HideInInspector]
    public RectTransform canvas;

    protected override void Awake()
    {
        base.Awake();
        waxDropZone = transform.FindDeepChild("WaxDropZone").GetComponent<RectTransform>();
        peanutDropZone = transform.FindDeepChild("PeanutDropZone").GetComponent<RectTransform>();
        cam = Camera.main;
        canvas = GetComponent<RectTransform>();
    }

    public RectTransform FindDropTarget(CurrencyDropType currencyDropType)
    {
        switch (currencyDropType)
        {
            case CurrencyDropType.Wax:
                return waxDropZone;
            case CurrencyDropType.Peanut:
                return peanutDropZone;
            default:
                return null;
        }
    }
}
