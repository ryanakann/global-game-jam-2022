using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;
using TMPro;
using UnityEngine.SceneManagement;


public class SelectionController : Singleton<SelectionController>
{
    public SelectionObject currentSelectionObject, selectedObject;

    [HideInInspector]
    public Location currentLocation;

    public List<DetailsPanel> panels = new List<DetailsPanel>();

    public DetailsPanel edgePanel, locationPanel, clownPanel;

    public Animator buttonsAnim, levelSelectAnim;

    bool fueling;
    Vector3 originalCameraPos;
    Transform levelGeneration;

    protected override void Awake()
    {
        base.Awake();
        levelSelectAnim = GetComponent<Animator>();
        buttonsAnim = transform.FindDeepChild("LevelSelectUI").GetComponent<Animator>();
        levelGeneration = transform.FindDeepChild("LevelGeneration");
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

    public void DRIVE()
    {
        print("DRIIIIIVE");
        currentLocation.OccupyNeighbor((Location)selectedObject);
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

    public void FuelUp()
    {
        if (fueling)
            return;
        fueling = true;
        levelSelectAnim.SetBool("LevelSelect", false);
        StartCoroutine(CoFuelUp());
    }

    IEnumerator CoFuelUp()
    {
        var op = SceneManager.LoadSceneAsync("LevelTest", LoadSceneMode.Additive);
        while (!op.isDone)
        { yield return null; }
        originalCameraPos = LevelGenerator.instance.cameraPivot.position;
        LevelGenerator.instance.cameraPivot.position = GameObject.Find("CameraPivot").transform.position;
        levelGeneration.gameObject.SetActive(false);
    }

    public void Scram()
    {
        if (!fueling)
            return;
        fueling = false;
        Destroy(GameObject.Find("Lanes"));
        StartCoroutine(CoScram());
    }

    IEnumerator CoScram()
    {
        var op = SceneManager.UnloadSceneAsync("LevelTest");
        while (!op.isDone)
        { yield return null; }
        levelSelectAnim.SetBool("LevelSelect", true);
        levelGeneration.gameObject.SetActive(true);
        LevelGenerator.instance.cameraPivot.position = originalCameraPos;
    }
}
