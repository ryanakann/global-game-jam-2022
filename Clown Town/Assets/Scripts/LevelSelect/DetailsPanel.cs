using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public enum PanelDetailsType { Text, Image, Button };

[System.Serializable]
public class PanelDetailsTuple
{
    public string elementName;
    public PanelDetailsType panelDetailsType;
}

public class DetailsPanel : MonoBehaviour
{

    public List<PanelDetailsTuple> elementsDict = new List<PanelDetailsTuple>();

    public Dictionary<string, PanelDetailsType> elementsTypeMap = new Dictionary<string, PanelDetailsType>();
    public Dictionary<string, GameObject> elementsMap = new Dictionary<string, GameObject>();

    // Start is called before the first frame update
    void Awake()
    {
        foreach (PanelDetailsTuple p in elementsDict)
        {
            elementsMap[p.elementName] = transform.FindDeepChild(p.elementName).gameObject;
            elementsTypeMap[p.elementName] = p.panelDetailsType;
        }
    }

    public void FillText(string elementName, string value)
    {
        elementsMap[elementName].GetComponent<TextMeshProUGUI>().text = value;
    }

    public void FillButton(string elementName, bool value)
    {
        elementsMap[elementName].SetActive(value);
    }

    public void FillImage(string elementName, Sprite image)
    {
        elementsMap[elementName].GetComponent<Image>().sprite = image;
    }
}
