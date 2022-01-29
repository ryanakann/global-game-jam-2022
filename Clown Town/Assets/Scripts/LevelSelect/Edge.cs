using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Edge : SelectionObject
{
    public Location src, tgt;

    public override void Activate()
    {
        base.Activate();
        tgt.Activate();
    }
}
