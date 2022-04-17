using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CurrencyTuple
{
    public CurrencyDropType CurrencyDropType;
    public GameObject drop;
}

[System.Serializable]
public class CurrencyDropTuple
{
    public List<CurrencyTuple> drops = new List<CurrencyTuple>();
    public float prob;
}

[CreateAssetMenu(fileName = "CurrencyDropperProfile", menuName = "ScriptableObjects/Currency Dropper Profile", order = 1)]
public class CurrencyDropperProfile : ScriptableObject
{
    CurrencyDropTuple defaultDrop;

    public List<CurrencyDropTuple> probsList = new List<CurrencyDropTuple>();
    public Dictionary<CurrencyDropTuple, float> probsDict = new Dictionary<CurrencyDropTuple, float>();

    private void OnEnable()
    {
#if UNITY_EDITOR
        // use platform dependent compilation so it only exists in editor, otherwise it'll break the build
        if (UnityEditor.EditorApplication.isPlayingOrWillChangePlaymode)
            Init();
#endif
    }

    private void Awake()
    {
        Init();
    }

    void Init()
    {
        defaultDrop = new CurrencyDropTuple();
        foreach (var p in probsList)
        {
            probsDict[p] = p.prob;
        }
    }

    public List<CurrencyTuple> GetDrop()
    {
        var drop = probsDict.Choose(defaultDrop);
        return drop.drops;
    }
}
