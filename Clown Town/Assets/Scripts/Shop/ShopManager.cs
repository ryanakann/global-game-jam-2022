using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : Singleton<ShopManager>
{

    private Unit placeholder;

    public void CreatePlaceholder(GameObject obj)
    {
        if (placeholder != null) return;

        placeholder = Instantiate(obj).GetComponent<Unit>();
    }

    public void RemovePlaceholder()
    {
        if (placeholder == null) return;
        Destroy(placeholder);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (placeholder != null)
            {
                if (placeholder.cost <= CurrencyManager.instance.currencyList[0].amount)
                {
                    LevelManager.instance.PlaceUnit(placeholder.gameObject, true);
                }
            }
        }
        else if (Input.GetMouseButtonDown(1))
        {
            if (placeholder != null)
            {
                RemovePlaceholder();
            }
        }
    }
}
