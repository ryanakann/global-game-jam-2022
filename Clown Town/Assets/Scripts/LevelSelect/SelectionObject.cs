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

    public SpriteRenderer selectHighlight, highlight;


    public virtual void Awake()
    {
        selectionState = new SelectionState();
        selectHighlight = transform.FindDeepChild("SelectHighlight").GetComponent<SpriteRenderer>();
        highlight = transform.FindDeepChild("Highlight").GetComponent<SpriteRenderer>();
    }

    public virtual void FillDetailsPanel()
    {

    }

    public virtual void Deactivate()
    {
        selectionState.canSelect = false;
        // TODO: fade color
        foreach (var r in GetComponentsInChildren<SpriteRenderer>())
        {
            r.color = new Color(r.color.r, r.color.g, r.color.b, 0.33f);
        }
        highlight.gameObject.SetActive(false);
        selectHighlight.gameObject.SetActive(false);
    }

    public virtual void Activate()
    {
        selectionState.canSelect = true;
        // TODO: set color
        foreach (var r in GetComponentsInChildren<SpriteRenderer>())
        {
            r.color = new Color(r.color.r, r.color.g, r.color.b, 1f);
        }
    }

    public virtual void Occupy()
    {
        selectionState.occupied = true;
        // TODO: add highlight
    }

    public virtual void Highlight()
    {
        highlight.color = new Color(highlight.color.r, highlight.color.g, highlight.color.b, 0.5f);
        highlight.gameObject.SetActive(true);
        // add additive highlight
    }

    public virtual void Unhighlight()
    {
        SelectionController.instance.ClearPanels(true);
        highlight.gameObject.SetActive(false);
        // remove additive highlight
    }

    public virtual void Select()
    {
        selectHighlight.gameObject.SetActive(true);
    }

    public virtual void Deselect(bool clear = true)
    {
        if (selectionState.occupied == true)
            return;
        if (clear)
            SelectionController.instance.ClearPanels();
        selectHighlight.gameObject.SetActive(false);
        // TODO: remove highlight
    }

    public virtual void Remove()
    {
        selectionState.alive = false;
        // TODO: remove highlight
        // TODO: complete fade
        foreach (var r in GetComponentsInChildren<SpriteRenderer>())
        {
            r.color = new Color(r.color.r, r.color.g, r.color.b, 0.1f);
        }
    }
}
