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

    public override void FillDetailsPanel()
    {
        base.FillDetailsPanel();
        SelectionController.instance.edgePanel.FillText("EdgeName", edgeName);
        SelectionController.instance.edgePanel.FillText("EdgeCost", $"Fuel Cost: {fuelCost}");
    }

    public override void Select()
    {
        base.Select();
        if (SelectionController.instance.ActivatePanel(SelectionController.instance.edgePanel, select: true))
            FillDetailsPanel();
    }

    public override void Highlight()
    {
        base.Highlight();
        if (SelectionController.instance.ActivatePanel(SelectionController.instance.edgePanel, select: false))
            FillDetailsPanel();
    }
}
