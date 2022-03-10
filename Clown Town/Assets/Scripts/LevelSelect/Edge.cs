using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Edge : SelectionObject
{
    public Location src, tgt;

    public string edgeName;
    public int fuelCost;

    public override void Awake()
    {
        base.Awake();
        selectable = false;
    }

    public override void Activate()
    {
        base.Activate();
        tgt.Activate();
    }
}
