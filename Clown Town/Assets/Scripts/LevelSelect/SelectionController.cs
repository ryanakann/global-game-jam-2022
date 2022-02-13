using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;
using TMPro;


public class SelectionController : Singleton<SelectionController>
{
    public SelectionObject currentSelectionObject, selectedObject;

    public List<DetailsPanel> panels = new List<DetailsPanel>();

    public DetailsPanel edgePanel, locationPanel, clownPanel;

    public Animator buttonsAnim;

    protected override void Awake()
    {
        base.Awake();
        buttonsAnim = transform.FindDeepChild("LevelSelectUI").GetComponent<Animator>();
    }

    public void Start()
    {
        panels = new List<DetailsPanel>(GetComponentsInChildren<DetailsPanel>());
        edgePanel = transform.FindDeepChild("EdgePanel").GetComponent<DetailsPanel>();
        locationPanel = transform.FindDeepChild("LocationPanel").GetComponent<DetailsPanel>();
        clownPanel = transform.FindDeepChild("ClownPanel").GetComponent<DetailsPanel>();
        foreach (var p in panels)
        {
            p.gameObject.SetActive(false);
        }
    }


    // Update is called once per frame
    void Update()
    {
        if (currentSelectionObject != null)
        {
            if (currentSelectionObject.selectionState.canHighlight == false)
            {
                currentSelectionObject.Unhighlight();
                currentSelectionObject = null;
            }
        }

        RaycastHit2D hit = Physics2D.Raycast(UtilsClass.GetMouseWorldPosition(), Vector3.forward);
        bool result = false;
        if (hit.collider != null)
        {
            var obj = hit.transform.GetComponentInParent<SelectionObject>();
            if (obj != null && obj.selectionState.canHighlight == true)
            {
                if (obj != currentSelectionObject)
                {
                    if (currentSelectionObject != null)
                    {
                        currentSelectionObject.Unhighlight();
                    }
                    currentSelectionObject = obj;
                    obj.Highlight();
                }
                result = true;
            }
        }
        else if (result == false && currentSelectionObject != null)
        {
            currentSelectionObject.Unhighlight();
            currentSelectionObject = null;
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (currentSelectionObject != null && currentSelectionObject.selectionState.canSelect == true && currentSelectionObject.selectable)
            {
                if (selectedObject != currentSelectionObject)
                {
                    if (selectedObject != null)
                        selectedObject.Deselect();
                    selectedObject = currentSelectionObject;
                    currentSelectionObject.Select();
                }
            }
        }
    }

    public bool ActivatePanel(DetailsPanel panel, bool select=false)
    {
        if (select || selectedObject == null)
        {
            foreach (var p in panels)
            {
                if (p == panel)
                    continue;
                p.gameObject.SetActive(false);
            }
            panel.gameObject.SetActive(true);
            return true;
        }
        return false;
    }

    public void ClearPanels(bool highlight=false)
    {
        if (highlight && selectedObject != null)
            return;

        buttonsAnim.SetBool("LocationOpen", false);
        buttonsAnim.SetBool("TalkOpen", false);
        if (selectedObject != null)
        {
            selectedObject.Deselect(false);
            selectedObject = null;
        }
        foreach (var p in panels)
        {
            p.gameObject.SetActive(false);
        }
    }

    public void ButtonClose(string button)
    {
        buttonsAnim.SetBool(button, false);
    }

    public void Clicko()
    {
        int clownId = ((ClownDisplay)selectedObject).clown.Id;
        ClownManager.SayQuipInFlowchartForClownForEvent(clownId, EventTypes.ClownTalk);
    }
}
