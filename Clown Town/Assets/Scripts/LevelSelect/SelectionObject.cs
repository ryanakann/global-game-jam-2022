using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// public enum SelectionState { Activated, Deactivated, Selected, Unselectable, Select, Highlight, Unhighlight };

public class SelectionState
{
    bool _alive;
    public bool alive { set { _alive = value; } get { return _alive; } }
    bool _occupied;
    public bool occupied { set { _occupied = value; } get { return _occupied && alive; } }

    bool _canSelect;
    public bool canSelect { set { _canSelect = value; } get { return _canSelect && !occupied && alive; } }
    bool _canHighlight;
    public bool canHighlight { set { _canHighlight = value; } get { return _canHighlight && (canSelect || occupied); } }

    public SelectionState()
    {
        alive = true;
        occupied = false;
        canSelect = true;
        canHighlight = true;
    }
}


public class SelectionObject : MonoBehaviour
{
    public SelectionState selectionState;

    UnityEvent<SelectionState> selectionEvent;

    private void Awake()
    {
        selectionState = new SelectionState();
    }

    public virtual void Deactivate()
    {
        selectionState.canSelect = false;
        // TODO: fade color
        print("DEACTIVATE ME");
    }

    public virtual void Activate()
    {
        selectionState.canSelect = true;
        // TODO: set color
        print("ACTIVATE ME");
    }

    public virtual void Occupy()
    {
        selectionState.occupied = true;
        // TODO: add highlight
    }

    public virtual void Highlight()
    {
        if (!selectionState.canHighlight)
            return;
        // TODO: up color
        print("VIEW ME!!!");
    }

    public virtual void Unhighlight()
    {
        if (!selectionState.canHighlight)
            return;
        // TODO: set color
        print("FORGET ME");
    }

    public virtual void Select()
    {
        if (!selectionState.canSelect)
            return;
        if (LevelGenerator.instance.currentSelectedObject != null)
            LevelGenerator.instance.currentSelectedObject.Deselect();
        // TODO: add highlight
        LevelGenerator.instance.currentSelectedObject = this;
        print("SELECT ME"); 
    }

    public virtual void Deselect()
    {
        if (!selectionState.canSelect)
            return;
        print("DESELECT ME");
        // TODO: remove highlight
    }

    public virtual void Remove()
    {
        print("KILL ME");
        selectionState.alive = false;
        // TODO: remove highlight
        // TODO: complete fade
    }

    private void OnMouseUpAsButton()
    {
        Select();
    }

    private void OnMouseEnter()
    {
        Highlight();
    }

    private void OnMouseExit()
    {
        Unhighlight();
    }
}
