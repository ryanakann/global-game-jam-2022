using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrencyManager : PersistentSingleton<CurrencyManager>
{
    public List<Currency> currencyList;

    private void OnEnable()
    {
        currencyList.ForEach(c => c.Setup());
    }

    private void OnDisable()
    {
        currencyList.ForEach(c => c.Teardown());
    }
}
