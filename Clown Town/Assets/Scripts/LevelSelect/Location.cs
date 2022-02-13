using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Location : SelectionObject
{
    [HideInInspector]
    public List<Edge> outgoingConnections = new List<Edge>();

    [HideInInspector]
    public List<Edge> ingoingConnections = new List<Edge>();

    public LocationType locationType;

    public string locationName, description;
    public int difficulty;

    public void SetLocationType(LocationType locType)
    {
        locationType = locType;
        locationName = locType.locationNames[Random.Range(0, locType.locationNames.Length)];
        description = locType.locationDescriptions[Random.Range(0, locType.locationDescriptions.Length)];
        GetComponentInChildren<SpriteRenderer>().sprite = locType.locationImages[Random.Range(0, locType.locationImages.Length)];
    }

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

    public override void FillDetailsPanel()
    {
        base.FillDetailsPanel();
        SelectionController.instance.locationPanel.FillText("LocationName", locationName);
        SelectionController.instance.locationPanel.FillText("LocationDescription", description);
        SelectionController.instance.locationPanel.FillText("LocationDifficulty", $"Difficulty: {difficulty}");
    }

    public override void Select()
    {
        base.Select();
        if (SelectionController.instance.ActivatePanel(SelectionController.instance.locationPanel, select: true))
        {
            FillDetailsPanel();
            SelectionController.instance.locationPanel.FillButton("LocationOpen", true);
        }
    }

    public override void Highlight()
    {
        base.Highlight();
        if (SelectionController.instance.ActivatePanel(SelectionController.instance.locationPanel, select: false))
        {
            FillDetailsPanel();
            SelectionController.instance.locationPanel.FillButton("LocationOpen", false);
        }
    }
}
