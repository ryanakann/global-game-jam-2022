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

    [HideInInspector]
    public bool selectable = true;

    public virtual void Awake()
    {
        selectionState = new SelectionState();
    }

    public virtual void FillDetailsPanel()
    {

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
        print("VIEW ME!!!");
    }

    public virtual void Unhighlight()
    {
        SelectionController.instance.ClearPanels(true);
        print("FORGET ME");
    }

    public virtual void Select()
    {
        print("SELECT ME"); 
    }

    public virtual void Deselect(bool clear = true)
    {
        if (clear)
            SelectionController.instance.ClearPanels();
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
}
