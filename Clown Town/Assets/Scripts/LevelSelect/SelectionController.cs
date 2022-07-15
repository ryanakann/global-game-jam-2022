using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Encounters;

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

    [HideInInspector]
    public bool fueling; 
    bool scramming;
    Vector3 originalCameraPos;
    Transform levelGeneration;

    [HideInInspector]
    public Button refuelButton;

    TextMeshProUGUI peanutCount;
    public int peanuts;
    [HideInInspector]
    public int basePeanuts = 10;

    TextMeshProUGUI waxCount;
    RectTransform waxBar;
    bool waxFlashing;
    [HideInInspector]
    public int wax;
    int maxWax = 12;

    Image unitIcon;
    UnitCell unitCell;
    int mask;

    Transform clownHolder, lowClownPoint, clownTalk, locationDeselect, scramButton, animalPanimal, clownClock;

    public bool canSelect = true;

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
        UpdateWax(0, start:true);
        refuelButton = transform.FindDeepChild("LeverHandle").GetComponent<Button>();
        peanutCount = transform.FindDeepChild("PeanutCount").GetComponent<TextMeshProUGUI>();
        unitIcon = transform.FindDeepChild("UnitIcon").GetComponent<Image>();
        clownHolder = transform.FindDeepChild("ClownDisplayHolder");
        lowClownPoint = transform.FindDeepChild("LowClownPoint");

        clownTalk = transform.FindDeepChild("ClownTalkButton");
        locationDeselect = transform.FindDeepChild("LocationDeselectButton");
        scramButton = transform.FindDeepChild("ScramButton");
        animalPanimal = transform.FindDeepChild("AnimalPanimal");
        clownClock = transform.FindDeepChild("ClownClock");

        mask = ~LayerMask.GetMask("Unit");
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

    public void AddCurrency(CurrencyDropType currencyDropType)
    {
        switch (currencyDropType)
        {
            case CurrencyDropType.Peanut:
                UpdatePeanuts(1);
                break;
            case CurrencyDropType.Wax:
                UpdateWax(1);
                break;
            default:
                break;
        }
    }

    public void UpdateBasePeanuts(int _peanuts)
    {
        instance.basePeanuts += _peanuts;
        FX_Spawner.instance.SpawnFX((Mathf.Sign(_peanuts) > -1) ? FXType.PeanutGain : FXType.PeanutLoss, Vector3.zero, Quaternion.identity);
        if (_peanuts > 0)
            ExplainerManager.Explain(Cue.BasePeanutGain);
    }

    public void UpdatePeanuts(int _peanuts)
    {
        peanuts += _peanuts;
        peanutCount.text = $"{peanuts}";
        FX_Spawner.instance.SpawnFX((Mathf.Sign(_peanuts) > -1) ? FXType.PeanutGain : FXType.PeanutLoss, Vector3.zero, Quaternion.identity);
    }


    public void UpdateWax(int _wax, bool start=false)
    {
        wax = Mathf.Clamp(wax + _wax, 0, maxWax);
        waxCount.text = $"WAX: {wax}";
        waxBar.anchorMax = new Vector2(wax / (float)maxWax, 1f);
        waxBar.offsetMax = new Vector2(0, waxBar.offsetMax.y);
        FX_Spawner.instance.SpawnFX((Mathf.Sign(_wax) > -1) ? FXType.WaxGain : FXType.WaxLoss, Vector3.zero, Quaternion.identity);

        if (!start)
        {
            if (_wax == maxWax)
                ExplainerManager.Explain(Cue.FullWax);
            else if (_wax > 0)
                ExplainerManager.Explain(Cue.GainWax);
        }

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
        if (ExplainerManager.instance != null && ExplainerManager.instance.explaining)
            return;

        unitIcon.transform.position = Input.mousePosition;

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

        if (!canSelect)
            return;

        if (currentSelectionObject != null)
        {
            if (currentSelectionObject.selectionState.canHighlight == false)
            {
                FX_Spawner.instance.SpawnFX(FXType.MouseOvernt, Vector3.zero, Quaternion.identity);
                currentSelectionObject.Unhighlight();
                currentSelectionObject = null;
            }
        }

        Gift giftHit = null;
        RaycastHit2D hit = Physics2D.Raycast(UtilsClass.GetMouseWorldPosition(), Vector3.forward, 10, mask);
        bool result = false;
        if (hit.collider != null)
        {
            SelectionObject obj;
            Gift tempGiftHit = hit.transform.GetComponent<Gift>();
            if (tempGiftHit != null && tempGiftHit.clicked == false)
            {
                giftHit = tempGiftHit;
            }

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
            FX_Spawner.instance.SpawnFX(FXType.MouseOvernt, Vector3.zero, Quaternion.identity);
            currentSelectionObject.Unhighlight();
            currentSelectionObject = null;
        }

        if (Input.GetMouseButtonDown(1))
        {
            if (fueling && unitCell != null)
            {
                unitCell = null;
                unitIcon.gameObject.SetActive(false);
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (fueling)
            {
                if (giftHit != null)
                {
                    giftHit.Click();
                }
                else if (unitCell != null)
                {
                    Vector2 pos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
                    if (EncounterBehavior.instance._info.IsPointWithinGrid(pos))
                    {
                        // check cost and placement
                        if (unitCell.cost <= peanuts)
                        {
                            if (EncounterBehavior.instance.encounterUnits.AddAllyUnit(unitCell.unitInfo, false, EncounterBehavior.instance._info.WorldToRoundedGridPosition(pos)))
                            {
                                UpdatePeanuts(-unitCell.cost);
                                unitCell = null;
                                unitIcon.gameObject.SetActive(false);
                            }
                        }
                        else if (!waxFlashing)
                        {
                            // flash edge panel
                            FX_Spawner.instance.SpawnFX(FXType.FailSpawn, Vector3.zero, Quaternion.identity);
                            StartCoroutine(CoWaxFlash(peanutCount));
                        }
                    }
                }
            }

            if (currentSelectionObject != null && currentSelectionObject.selectionState.canSelect == true && currentSelectionObject.selectable)
            {
                if (selectedObject != currentSelectionObject)
                {
                    if (selectedObject != null)
                    {
                        selectedObject.Deselect();
                    }
                    selectedObject = currentSelectionObject;
                    currentSelectionObject.Select();
                }
            }
        }
    }

    public void SelectUnit(UnitCell cell)
    {
        unitCell = cell;
        unitIcon.gameObject.SetActive(true);
        unitIcon.sprite = cell.icon;
        ExplainerManager.Explain(Cue.UnitSelect);
    }

    public void DRIVE()
    {
        int cost = ((Location)selectedObject).activeEdge.fuelCost;
        if (wax - cost < 0)
        {
            // flash edge panel
            FX_Spawner.instance.SpawnFX(FXType.FailDrive, Vector3.zero, Quaternion.identity);
            ExplainerManager.Explain(Cue.FailDrive);
            if (!waxFlashing)
                StartCoroutine(CoWaxFlash(locationPanel.elementsMap["EdgeCost"].GetComponent<TextMeshProUGUI>()));
            return;
        }
        else
        {
            UpdateWax(-cost);
        }
        ExplainerManager.Explain(Cue.DRIVE);
        FX_Spawner.instance.SpawnFX(FXType.DRIVE, Vector3.zero, Quaternion.identity);
        MusicoManager.instance.AdvancePlayer();
        wheelAnim.SetTrigger("Spin");
        currentLocation.OccupyNeighbor((Location)selectedObject);
        ClearPanels();
    }


    IEnumerator CoWaxFlash(TextMeshProUGUI text)
    {
        waxFlashing = true;
        float t = 0, max = 0.75f;
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
        UpdatePeanuts(-peanuts);
        UpdatePeanuts(basePeanuts);
        if (fueling || scramming)
            return;
        FX_Spawner.instance.SpawnFX(FXType.FuelUp, Vector3.zero, Quaternion.identity);
        ExplainerManager.Explain(Cue.EnterBattlezone);
        refuelButton.interactable = false;
        fueling = true;
        levelSelectAnim.SetBool("LevelSelect", false);
        StartCoroutine(CoFuelUp());
    }

    IEnumerator CoFuelUp()
    {
        Wall.instance.Switch(false);
        while (Wall.instance.moving)
        { yield return null; }
        var op = SceneManager.LoadSceneAsync("BattleZone", LoadSceneMode.Additive);
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
        FX_Spawner.instance.SpawnFX(FXType.Scram, Vector3.zero, Quaternion.identity);
        unitCell = null;
        unitIcon.sprite = null;
        unitIcon.gameObject.SetActive(false);
        scramming = true;
        fueling = false;
        levelSelectAnim.SetBool("LevelSelect", true);
        ClearPanels();
        StartCoroutine(CoScram());
    }

    public void YouCanLeave()
    {
        FX_Spawner.instance.SpawnFX(FXType.TimerFinish, Vector3.zero, Quaternion.identity);
        ExplainerManager.Explain(Cue.BattlezoneTimerFinish);
    }

    public void ManPutThoseClownsDownThere()
    {
        clownHolder.parent = lowClownPoint;
        clownHolder.localPosition = Vector3.zero;
    }

    public void PutTheDamnClownsBack()
    {
        clownHolder.parent = ClownsDisplay.instance.transform;
        clownHolder.localPosition = Vector3.zero;
    }

    IEnumerator CoScram()
    {
        Wall.instance.Switch(false);
        while (Wall.instance.moving)
        { yield return null; }
        Destroy(GameObject.Find("Lanes"));
        var op = SceneManager.UnloadSceneAsync("BattleZone");
        while (!op.isDone)
        { yield return null; }
        levelGeneration.gameObject.SetActive(true);
        LevelGenerator.instance.cameraPivot.position = originalCameraPos;
        Wall.instance.Switch(true);
        while (Wall.instance.moving)
        { yield return null; }
        scramming = false;
        refuelButton.interactable = true;
        MusicoManager.instance.UpdateMusic();
    }

    static public Vector3 GetWaxCountPos()
    {
        return Helper.ScreenToWorld(instance.waxCount.rectTransform);
    }

    static public Vector3 GetRefuelLeverPos()
    {
        return Helper.ScreenToWorld(instance.refuelButton.GetComponent<RectTransform>());
    }

    static public Vector3 GetClownPanelPos()
    {
        return Helper.ScreenToWorld(instance.clownPanel.GetComponent<RectTransform>());
    }

    static public Vector3 GetClownTalkPos()
    {
        return Helper.ScreenToWorld(instance.clownTalk.GetComponent<RectTransform>());
    }

    static public Vector3 GetLocationDeselectPos()
    {
        return Helper.ScreenToWorld(instance.locationDeselect.GetComponent<RectTransform>());
    }

    static public Vector3 GetLocationWaxCostPos()
    {
        return Helper.ScreenToWorld(instance.locationPanel.elementsMap["EdgeCost"].GetComponent<RectTransform>());
    }

    static public Vector3 GetLocationDifficultyPos()
    {
        return Helper.ScreenToWorld(instance.locationPanel.elementsMap["LocationDifficulty"].GetComponent<RectTransform>());
    }

    static public Vector3 GetLocationDescriptionPos()
    {
        return Helper.ScreenToWorld(instance.locationPanel.elementsMap["LocationDescription"].GetComponent<RectTransform>());
    }

    static public Vector3 GetWheelPos()
    {
        return Helper.ScreenToWorld(instance.wheelAnim.GetComponent<RectTransform>());
    }

    static public Vector3 GetScramPos()
    {
        return Helper.ScreenToWorld(instance.scramButton.GetComponent<RectTransform>());
    }

    static public Vector3 GetPeanutScreenPos()
    {
        return Helper.ScreenToWorld(instance.peanutCount.GetComponent<RectTransform>());
    }

    static public Vector3 GetAnimalPanimalPos()
    {
        return Helper.ScreenToWorld(instance.animalPanimal.GetComponent<RectTransform>());
    }

    static public Vector3 GetTimerPos()
    {
        return Helper.ScreenToWorld(instance.clownClock.GetComponent<RectTransform>());
    }

    static public bool GetFueling()
    {
        return instance.fueling;
    }
}
