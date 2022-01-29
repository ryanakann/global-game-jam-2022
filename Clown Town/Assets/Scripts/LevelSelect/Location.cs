using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Location : SelectionObject
{
    [HideInInspector]
    public List<Edge> outgoingConnections = new List<Edge>();

    [HideInInspector]
    public List<Edge> ingoingConnections = new List<Edge>();

    public void OccupyNeighbor(Location target_loc)
    {
        foreach (var edge in outgoingConnections)
        {
            if (edge.tgt == target_loc)
            {
                edge.Remove();
                continue;
            }
            edge.tgt.Remove();
        }
        foreach (var edge in ingoingConnections)
        {
            edge.Remove();
        }
    }

    public override void Remove()
    {
        base.Remove();
        foreach (var edge in outgoingConnections)
            edge.Remove();

        foreach (var edge in ingoingConnections)
            edge.Remove();
    }

    public override void Occupy()
    {
        base.Occupy();
        foreach (var edge in outgoingConnections)
        {
            edge.Activate();
        }
    }

    public override void Deactivate()
    {
        base.Deactivate();
        foreach (var edge in ingoingConnections)
        {
            edge.Deactivate();
        }
    }
}
