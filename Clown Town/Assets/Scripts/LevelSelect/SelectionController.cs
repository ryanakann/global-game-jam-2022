using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;
using TMPro;
using UnityEngine.SceneManagement;


public class SelectionController : Singleton<SelectionController>
{
    [HideInInspector]
    public Camera mainCamera;

    public SelectionObject currentSelectionObject, selectedObject;

    [HideInInspector]
    public Location currentLocation;

    public List<DetailsPanel> panels = new List<DetailsPanel>();

    public DetailsPanel locationPanel, clownPanel;

    public Animator buttonsAnim, levelSelectAnim, wheelAnim;

    bool fueling;
    Vector3 originalCameraPos;
    Transform levelGeneration;

    TextMeshProUGUI waxCount;
    RectTransform waxBar;

    bool waxFlashing;

    [HideInInspector]
    public int wax;

    int maxWax = 12;

    protected override void Awake()
    {
        base.Awake();
        mainCamera = Camera.main;
        levelSelectAnim = GetComponent<Animator>();
        buttonsAnim = transform.FindDeepChild("LevelSelectUI").GetComponent<Animator>();
        levelGeneration = transform.FindDeepChild("LevelGeneration");
        waxCount = transform.FindDeepChild("WaxText").GetComponent<TextMeshProUGUI>();
        waxBar = transform.FindDeepChild("CurrentWax").GetComponent<RectTransform>();
        wheelAnim = transform.FindDeepChild("Wheel").GetComponent<Animator>();
        UpdateWax(0);
    }

    public void Start()
    {
        panels = new List<DetailsPanel>(GetComponentsInChildren<DetailsPanel>());
        locationPanel = transform.FindDeepChild("LocationPanel").GetComponent<DetailsPanel>();
        clownPanel = transform.FindDeepChild("ClownPanel").GetComponent<DetailsPanel>();
        foreach (var p in panels)
        {
            p.gameObject.SetActive(false);
        }
    }

    public void UpdateWax(int _wax)
    {
        wax = Mathf.Clamp(wax + _wax, 0, maxWax);
        waxCount.text = $"WAX: {wax}";
        waxBar.anchorMax = new Vector2(wax / (float)maxWax, 1f);
        waxBar.offsetMax = new Vector2(0, waxBar.offsetMax.y); ;
        if (wax == 0)
            waxCount.color = Color.red;
        else if (wax == maxWax)
            waxCount.color = Color.green;
        else
            waxCount.color = Color.white;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            UpdateWax(1);
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            ClownManager.DamageClowns(10);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            ClownManager.DamageClowns(1);
        }

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
            SelectionObject obj;
            if (hit.transform.GetComponentInParent<Edge>())
            {
                obj = hit.transform.GetComponentInParent<Edge>().tgt;
            }
            else
            {
                obj = hit.transform.GetComponentInParent<SelectionObject>();
            }
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
        if (result == false && currentSelectionObject != null)
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
        int cost = ((Location)selectedObject).activeEdge.fuelCost;
        if (wax - cost < 0 && !waxFlashing)
        {
            // flash edge panel
            StartCoroutine(CoWaxFlash());
            return;
        }
        else
        {
            UpdateWax(-cost);
        }
        wheelAnim.SetTrigger("Spin");
        currentLocation.OccupyNeighbor((Location)selectedObject);
        ClearPanels();
    }

    IEnumerator CoWaxFlash()
    {
        waxFlashing = true;
        float t = 0, max = 0.75f;
        TextMeshProUGUI text = locationPanel.elementsMap["EdgeCost"].GetComponent<TextMeshProUGUI>();
        while (t < max)
        {
            t += Time.deltaTime;
            text.color = Color.Lerp(Color.white, Color.red, Mathf.Sin(t/max * Mathf.PI));
            yield return null;
        }
        waxFlashing = false;
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
        Wall.instance.Switch(false);
        while (Wall.instance.moving)
        { yield return null; }
        var op = SceneManager.LoadSceneAsync("LevelTest", LoadSceneMode.Additive);
        while (!op.isDone)
        { yield return null; }
        originalCameraPos = LevelGenerator.instance.cameraPivot.position;
        LevelGenerator.instance.cameraPivot.position = GameObject.Find("CameraPivot").transform.position;
        levelGeneration.gameObject.SetActive(false);
        Wall.instance.Switch(true);
        while (Wall.instance.moving)
        { yield return null; }
    }

    public void Scram()
    {
        if (!fueling)
            return;
        fueling = false;
        levelSelectAnim.SetBool("LevelSelect", true);
        StartCoroutine(CoScram());
    }

    IEnumerator CoScram()
    {
        Wall.instance.Switch(false);
        while (Wall.instance.moving)
        { yield return null; }
        Destroy(GameObject.Find("Lanes"));
        var op = SceneManager.UnloadSceneAsync("LevelTest");
        while (!op.isDone)
        { yield return null; }
        levelGeneration.gameObject.SetActive(true);
        LevelGenerator.instance.cameraPivot.position = originalCameraPos;
        Wall.instance.Switch(true);
        while (Wall.instance.moving)
        { yield return null; }
    }
}
