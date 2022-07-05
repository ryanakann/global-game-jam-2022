using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Location : SelectionObject
{
    [HideInInspector]
    public List<Edge> outgoingConnections = new List<Edge>();

    [HideInInspector]
    public List<Edge> ingoingConnections = new List<Edge>();

    [HideInInspector]
    public Edge activeEdge;

    public LocationType locationType;

    public string locationName, description;
    public int difficulty;

    [HideInInspector]
    public bool finalLocation;

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
                //edge.Remove();
                edge.Deactivate();
                continue;
            }
            edge.tgt.Remove();
        }
        foreach (var edge in ingoingConnections)
        {
            edge.Remove();
        }
        selectHighlight.gameObject.SetActive(false);
        target_loc.Occupy();

        if (!target_loc.finalLocation && target_loc.locationType.eventPrefabs.Length > 0)
        {
            Dialogue d = Instantiate(target_loc.locationType.eventPrefabs[Random.Range(0, target_loc.locationType.eventPrefabs.Length)]).GetComponent<Dialogue>();
            EventManager.instance.PushDialogue(d);
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
            edge.tgt.activeEdge = edge;
            edge.Activate();
        }
        base.Select();
        activeEdge = null;
        LevelGenerator.instance.UpdateCamera(transform.position);
        SelectionController.instance.currentLocation = this;

        if (finalLocation)
            FaderCanvas.instance.GoAway("Win");
    }

    public override void Deactivate()
    {
        base.Deactivate();
        foreach (var edge in ingoingConnections)
        {
            edge.Deactivate();
        }
    }

    public override void Deselect(bool clear = true)
    {
        base.Deselect(clear);
        if (activeEdge)
            activeEdge.Deselect(clear);
    }

    public override void Unhighlight()
    {
        base.Unhighlight();
        if (activeEdge)
            activeEdge.Unhighlight();
    }

    public override void FillDetailsPanel()
    {
        base.FillDetailsPanel();
        SelectionController.instance.locationPanel.FillText("LocationName", locationName);
        SelectionController.instance.locationPanel.FillText("LocationDescription", description);
        SelectionController.instance.locationPanel.FillText("LocationDifficulty", $"Difficulty: {difficulty}");
        if (activeEdge)
            SelectionController.instance.locationPanel.FillText("EdgeCost", $"Wax Cost: {activeEdge.fuelCost}");
    }

    public override void Select()
    {
        base.Select();
        if (activeEdge)
            activeEdge.Select();
        if (SelectionController.instance.ActivatePanel(SelectionController.instance.locationPanel, select: true))
        {
            FillDetailsPanel();
            SelectionController.instance.locationPanel.FillButton("LocationOpen", true);
        }
    }

    public override void Highlight()
    {
        base.Highlight();
        if (activeEdge)
            activeEdge.Highlight();
        if (SelectionController.instance.ActivatePanel(SelectionController.instance.locationPanel, select: false))
        {
            FillDetailsPanel();
            SelectionController.instance.locationPanel.FillButton("LocationOpen", false);
        }
    }
}
