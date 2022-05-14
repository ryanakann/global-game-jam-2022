using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Encounters;

public class UnitCell : MonoBehaviour
{
    public UnitInfo unitInfo;
    public Sprite icon;
    public int cost;

    // Start is called before the first frame update
    void Start()
    {
        GetComponentInChildren<TextMeshProUGUI>().text = cost.ToString();
    }

    public void Select()
    {
        SelectionController.instance.SelectUnit(this);
    }
}
