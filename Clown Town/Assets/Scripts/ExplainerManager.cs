using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using Encounters;

public delegate bool Pred();
public delegate Vector3 PosGetter();

public class Explanation
{
    public string text;
    public GameObject dialogueBox;
    public Vector3 lrStartPosition;
    public bool usePointer;
    public Pred predicate;
    public PosGetter lrEndPosGetter;
    public bool uiHighlight;

    public float delay;

    public Explanation(string text, GameObject dialogueBox, Vector3 lrStartPosition, PosGetter posGetter, bool usePointer=true, Pred predicate=null, float delay=0)
    {
        this.text = text;
        this.dialogueBox = dialogueBox;
        this.lrStartPosition = lrStartPosition;
        this.lrEndPosGetter = posGetter;
        this.usePointer = usePointer;
        this.predicate = predicate;
        this.delay = delay;
    }

    public Explanation(string text, GameObject dialogueBox, Pred predicate=null, float delay=0)
    {
        this.text = text;
        this.dialogueBox = dialogueBox;
        this.predicate = predicate;
        this.delay = delay;
    }

    public Explanation(string text, GameObject dialogueBox, PosGetter posGetter, Pred predicate = null, float delay=0)
    {
        this.text = text;
        this.dialogueBox = dialogueBox;
        this.predicate = predicate;
        uiHighlight = true;
        this.lrEndPosGetter = posGetter;
        this.delay = delay;
    }
}

public enum Cue 
{
    ClownIntro,
    StartGame,
    MouseOverClown,
    ClownSelect,
    MouseOverLocation,
    LocationSelect,

    DRIVE,
    EnterBattlezone,
    BattlezoneTimerFinish,

    BasePeanutGain, // pred
    ClownHarm, // pred
    ClownDie,
    GainWax, 
    FullWax, // pred, pred
    GiftSpawn,
    UnitSelect, //
    FailDrive, //
};


public class ExplainerManager : PersistentSingleton<ExplainerManager>
{

    Transform[] uiPivots = new Transform[9];
    Vector2[] lrPivots = new Vector2[9];
    LineRenderer lr;
    Transform arrowHead;


    GameObject blockerCanvas;

    List<Explanation> explainerQueue = new List<Explanation>();

    Dictionary<Cue, List<Explanation>> explainerMap;

    HashSet<Cue> explainedSet = new HashSet<Cue>();

    [HideInInspector]
    public List<ClownProfile> clowns = new List<ClownProfile>();

    [HideInInspector]
    public bool explaining;
    [HideInInspector]
    public bool explain;

    RectTransform eyeHighlight;

    bool added;
    RectTransform canvasRect;

    float currentDelay = 0;
    bool EXPLAINIT;

    protected override void Awake()
    {
        base.Awake();
        canvasRect = transform.FindDeepChild("BlockerCanvas").GetComponent<RectTransform>();
        arrowHead = transform.FindDeepChild("arrow_head");

        eyeHighlight = transform.FindDeepChild("HighlightEye").GetComponent<RectTransform>();

        // initialize pivots
        lr = GetComponent<LineRenderer>();

        lrPivots[0] = transform.FindDeepChild("topLeftPivot").position;
        lrPivots[1] = transform.FindDeepChild("topMidPivot").position;
        lrPivots[2] = transform.FindDeepChild("topRightPivot").position;
        lrPivots[3] = transform.FindDeepChild("midLeftPivot").position;
        lrPivots[4] = transform.FindDeepChild("midMidPivot").position;
        lrPivots[5] = transform.FindDeepChild("midRightPivot").position;
        lrPivots[6] = transform.FindDeepChild("bottomLeftPivot").position;
        lrPivots[7] = transform.FindDeepChild("bottomMidPivot").position;
        lrPivots[8] = transform.FindDeepChild("bottomRightPivot").position;

        uiPivots[0] = transform.FindDeepChild("topLeft");
        uiPivots[1] = transform.FindDeepChild("topMid");
        uiPivots[2] = transform.FindDeepChild("topRight");
        uiPivots[3] = transform.FindDeepChild("midLeft");
        uiPivots[4] = transform.FindDeepChild("midMid");
        uiPivots[5] = transform.FindDeepChild("midRight");
        uiPivots[6] = transform.FindDeepChild("bottomLeft");
        uiPivots[7] = transform.FindDeepChild("bottomMid");
        uiPivots[8] = transform.FindDeepChild("bottomRight");

        blockerCanvas = transform.FindDeepChild("BlockerCanvas").gameObject;

        explainerMap =
            new Dictionary<Cue, List<Explanation>>()
            {
                {Cue.ClownIntro, new List<Explanation>()
                    { 
                        new Explanation("Welcome to CLOWN HELL PSYCHOPOMP! (Click to clowntinue)", uiPivots[4].gameObject),
                        new Explanation("You are the Underworld Guide of hopeless, deceased clowns on their journey to the FINAL CIRCUS.", uiPivots[4].gameObject),
                        new Explanation("Time to meet the clowns...", uiPivots[4].gameObject),
                    }
                },
                {Cue.StartGame, new List<Explanation>()
                    {
                        new Explanation("This is your CLOWN CAR. The hub of all your actions.", uiPivots[1].gameObject),
                        new Explanation("These are your clowns. You can mouse over them to view information about them on the computer.", 
                            uiPivots[7].gameObject, ClownsDisplay.GetClownsDisplayPos), //lrPivots[7], 
                        new Explanation("This is the map. Locations are represented by those little icons. Edges indicate whether one location can be visited from another.", 
                            uiPivots[8].gameObject, LevelGenerator.GetStartLocationPos), //lrPivots[8], 
                        new Explanation("If you mouse over a location or an edge, useful information will appear on the computer.", 
                            uiPivots[8].gameObject),
                        new Explanation("This is your WAX METER. Like most clown cars, it is fueled by wax.", 
                            uiPivots[5].gameObject, SelectionController.GetWaxCountPos), // lrPivots[5],
                        new Explanation("You can collect wax by pulling this lever to enter the BATTLEZONE... More will be explained once you click on that.", 
                            uiPivots[5].gameObject, SelectionController.GetRefuelLeverPos), // lrPivots[5], 
                        new Explanation("The amount of wax required to traverse to a location can be viewed on the computer, but be warned! Each location has some degree of difficulty associated with it.", 
                            uiPivots[4].gameObject),
                        new Explanation("This is the radio. You can play/pause by clicking on this button.", 
                            uiPivots[4].gameObject, Radio.GetPos), // lrPivots[4],
                        new Explanation("These buttons control the volume.", 
                            uiPivots[4].gameObject, Radio.GetVolumePos), // lrPivots[4], 
                        new Explanation("This button allows you change the channel.", 
                            uiPivots[4].gameObject, Radio.GetSkipPos), // lrPivots[4], 
                        new Explanation("Speaking of music... You are being pursued by a wave of demonic instruments. THE MUSIC. Every time you choose to collect wax, the MUSIC will incrementally advance. If it overtakes you, YOU LOSE.", 
                            uiPivots[2].gameObject, MusicoManager.GetPos), // lrPivots[2], 
                        new Explanation("Your goal is to traverse to the very end with at least one 'living' clown, where a mighty fine SURPRISE awaits you.", uiPivots[1].gameObject),
                        new Explanation("By the way, press P to pause.", uiPivots[1].gameObject),
                        new Explanation("GOOD LUCK!", uiPivots[1].gameObject),
                    }
                },
                {Cue.MouseOverClown, new List<Explanation>()
                    {
                        new Explanation("Here you can see your clown's name, health, and personality.", 
                            uiPivots[4].gameObject, SelectionController.GetClownPanelPos), // lrPivots[4], 
                        new Explanation("Try clicking the clown to SELECT the clown.", uiPivots[4].gameObject),
                    }
                },
                {Cue.ClownSelect, new List<Explanation>()
                    {
                        new Explanation("In addition to locking the computer screen, you now have the option to talk to or deselect the clown.", 
                            uiPivots[4].gameObject, SelectionController.GetClownTalkPos, delay:0.75f), // lrPivots[4], 
                    }
                },
                {Cue.MouseOverLocation, new List<Explanation>()
                    {
                        new Explanation("Here you can see the location name, description, difficulty, and wax cost.", 
                            uiPivots[4].gameObject, SelectionController.GetClownPanelPos), // lrPivots[4], 
                        new Explanation("Try clicking the location to select it.", 
                            uiPivots[4].gameObject),
                    }
                },
                {Cue.LocationSelect, new List<Explanation>()
                    {
                        new Explanation("The wax cost indicates how much wax you will need to consume to traverse to that location.", 
                            uiPivots[4].gameObject, SelectionController.GetLocationWaxCostPos, delay:0.75f), // lrPivots[4],
                        new Explanation("The difficulty indicates how hard it will be to collect wax in this zone.", 
                            uiPivots[4].gameObject, SelectionController.GetLocationDifficultyPos), // lrPivots[4],
                        new Explanation("And, finally, the description gives you some notion of what events you can expect at that location.", 
                            uiPivots[4].gameObject, SelectionController.GetLocationDescriptionPos), // lrPivots[4],
                        new Explanation("If you have enough wax, click on the wheel to DRIVE!", 
                            uiPivots[4].gameObject, SelectionController.GetWheelPos), // lrPivots[4],
                        new Explanation("Otherwise, click the SCRAM button to deselect.", 
                            uiPivots[4].gameObject, SelectionController.GetLocationDeselectPos), // lrPivots[4],
                    }
                },
                {Cue.DRIVE, new List<Explanation>()
                    {
                        new Explanation("VROOM VROOM! You've taken your first clown steps. Brace for an event.", uiPivots[4].gameObject, delay:0.5f),
                    }
                },
                {Cue.EnterBattlezone, new List<Explanation>()
                    {
                        // peanuts
                        new Explanation("Welcome to the Battlezone!", uiPivots[4].gameObject),
                        new Explanation("These are your PEANUTS. Like most CLOWN HELL PSYCHOPOMPS, you use peanuts to summon your units. (Peanuts regenerate at the start of every Battlezone encounter)", 
                            uiPivots[4].gameObject, SelectionController.GetPeanutScreenPos), // lrPivots[4],
                        // units
                        new Explanation("Your units, of course, are circus animals, like dogs, elephants, and monkeys. Click on them to select a unit and then click on the grid to place the unit.", 
                            uiPivots[4].gameObject, SelectionController.GetAnimalPanimalPos), // lrPivots[4],
                        // spawners
                        new Explanation("These are evil spawners of demonic instruments.", 
                            uiPivots[4].gameObject, EncounterBehavior.GetSpawnerPos), // lrPivots[4],
                        // tents
                        new Explanation("The instruments will move from right to left to damage your precious clowns. Every instrument that reaches these tents will deliver some damage.", 
                            uiPivots[4].gameObject, ClownZone.GetTentPos), // lrPivots[4],
                        // drops
                        new Explanation("Killing enemies and clicking gifts (more on that later) will drop peanuts and wax.", uiPivots[4].gameObject),
                        // timer
                        new Explanation("Once you have enough wax and this timer has expired...", 
                            uiPivots[4].gameObject, SelectionController.GetTimerPos), // lrPivots[4],
                        new Explanation("You can click the SCRAM button to skeedaddle.", 
                            uiPivots[4].gameObject, SelectionController.GetScramPos), // lrPivots[4],
                    }
                },
                {Cue.BattlezoneTimerFinish, new List<Explanation>()
                    {
                        new Explanation("You've lasted long enough. You can exit whenever by clicking the SCRAM button", 
                            uiPivots[4].gameObject, SelectionController.GetScramPos), // lrPivots[4],
                    }
                },
                {Cue.BasePeanutGain, new List<Explanation>()
                    {
                        new Explanation("Your base peanut count has increased! Next time you're in the Battlezone, you'll have more peanuts at the start.",
                            uiPivots[4].gameObject, SelectionController.GetScramPos, predicate:delegate{ return instance.explainedSet.Contains(Cue.EnterBattlezone); }), // lrPivots[4],
                        new Explanation("You just got some extra peanuts! When you enter the Battlezone, this will become important.",
                            uiPivots[4].gameObject, SelectionController.GetScramPos, predicate:delegate{ return !instance.explainedSet.Contains(Cue.EnterBattlezone); }), // lrPivots[4],
                    }
                },
                {Cue.ClownHarm, new List<Explanation>()
                    {
                        new Explanation("Youch! A clown has been hurt. Too much damage and a clown will die!",
                            uiPivots[4].gameObject, predicate:delegate{ return !instance.explainedSet.Contains(Cue.ClownDie) && SelectionController.GetFueling(); }),
                        new Explanation("Oof. A clown just took some damage. Too much damage and they shall perish.",
                            uiPivots[4].gameObject, ClownsDisplay.GetClownsDisplayPos, 
                                predicate:delegate{ return !instance.explainedSet.Contains(Cue.ClownDie) && !SelectionController.GetFueling(); }), // lrPivots[4],
                    }
                },
                {Cue.ClownDie, new List<Explanation>()
                    {
                        new Explanation("Oh no! A clown has been obliterated. (There is no world beyond Clown Hell, unfortunately.)",
                            uiPivots[4].gameObject, predicate:delegate{ return SelectionController.GetFueling(); }),
                        new Explanation("Yikes! There is nothing sadder than a dead clown. Except maybe two dead clowns. Be careful.",
                            uiPivots[4].gameObject, ClownsDisplay.GetClownsDisplayPos,
                                predicate:delegate{ return !SelectionController.GetFueling(); }), // lrPivots[4],
                    }
                },
                {Cue.GainWax, new List<Explanation>()
                    {
                        new Explanation("Huzzah! You've gained some wax! You'll need that to travel to the next location.",
                            uiPivots[4].gameObject, SelectionController.GetWaxCountPos), // lrPivots[4],
                    }
                },
                {Cue.FullWax, new List<Explanation>()
                    {
                        new Explanation("You've got maximum waximum!",
                            uiPivots[4].gameObject, SelectionController.GetWaxCountPos), // lrPivots[4],
                    }
                },
                {Cue.GiftSpawn, new List<Explanation>()
                    {
                        new Explanation("Ah, lookie there. A gift! Click on it before it disappears to get a random drop!",
                            uiPivots[7].gameObject, GiftSpawner.GetLastGift), // lrPivots[7],
                    }
                },
                {Cue.UnitSelect, new List<Explanation>()
                    {
                        new Explanation("Great! You've just selected your first unit. I hope you can afford it. Click anywhere on the grid to place it.",
                            uiPivots[1].gameObject),
                    }
                },
                {Cue.FailDrive, new List<Explanation>()
                    {
                        new Explanation("Looks like you don't have enough wax to travel. Womp womp. You can always enter the Battlezone to collect some more.",
                            uiPivots[4].gameObject, SelectionController.GetLocationWaxCostPos), // lrPivots[4],
                    }
                },
            };
    }

    void SetHighlightPosition(Vector3 pos)
    {
        Vector2 ViewportPosition = Camera.main.WorldToViewportPoint(pos);
        Vector2 highlightWidth = (eyeHighlight.anchorMax - eyeHighlight.anchorMin) / 2.0f;
        eyeHighlight.anchorMin = ViewportPosition - highlightWidth;
        eyeHighlight.anchorMax = ViewportPosition + highlightWidth;
        /*
        Vector2 WorldObject_ScreenPosition = new Vector2(
        ((ViewportPosition.x * canvasRect.sizeDelta.x) - (canvasRect.sizeDelta.x * 0.5f)),
        ((ViewportPosition.y * canvasRect.sizeDelta.y) - (canvasRect.sizeDelta.y * 0.5f)));

        //now you can set the position of the ui element
        eyeHighlight.anchoredPosition = WorldObject_ScreenPosition;
        */
    }

    private void Update()
    {
        if (SelectionController.instance != null)
            SelectionController.instance.canSelect = explainerQueue.Count == 0;
        if (explaining && Input.GetMouseButtonDown(0))
        {
            Nextplanation();
        }
    }

    static public void Explain(Cue cue)
    {
        if (instance == null)
            return;
        instance.Explainst(cue);
    }

    public void Explainst(Cue cue)
    {
        if (!explain || explainedSet.Contains(cue))
            return;
        if (explainerMap.ContainsKey(cue) && !explainedSet.Contains(cue))
        {
            explainedSet.Add(cue);
            explainerQueue.AddRange(explainerMap[cue]);
        }

        if (!explaining && !added)
        {
            currentDelay = explainerQueue[0].delay;
            if (FaderCanvas.instance.fading)
            {
                FaderCanvas.instance.finishedFading += Execute;
                added = true;
            }
            else
            {
                Execute();
            }
        }
    }

    public void ResetExplainer()
    {
        explainedSet.Clear();
        explain = false;
        explaining = false;
        lr.enabled = false;
        arrowHead.gameObject.SetActive(false);
        if (explainerQueue.Count > 0)
        {
            explainerQueue[0].dialogueBox.SetActive(false);
            explainerQueue.Clear();
        }
        if (PauseManager.instance != null)
        {
            Time.timeScale = 1;
            PauseManager.instance.megaPaused = false;
        }
    }

    public void Nextplanation()
    {
        explainerQueue[0].dialogueBox.SetActive(false);
        lr.enabled = false;
        eyeHighlight.gameObject.SetActive(false);
        arrowHead.gameObject.SetActive(false);

        explainerQueue.RemoveAt(0);
        if (explainerQueue.Count == 0)
        {
            explaining = false;
            blockerCanvas.SetActive(false);
            Time.timeScale = 1;
            if (PauseManager.instance != null)
                PauseManager.instance.megaPaused = false;
            return;
        }
        currentDelay = 0;
        Execute();
    }

    void Execute()
    {
        StartCoroutine(CoExecute());
    }

    IEnumerator CoExecute()
    {
        if (explainerQueue[0].predicate != null && explainerQueue[0].predicate() == false)
            yield break;


        yield return new WaitForSeconds(currentDelay);

        blockerCanvas.SetActive(true);
        FaderCanvas.instance.finishedFading -= Execute;
        added = false;

        explaining = true;

        lr.enabled = explainerQueue[0].usePointer;
        arrowHead.gameObject.SetActive(explainerQueue[0].usePointer);

        if (explainerQueue[0].usePointer)
        {
            Vector3 pos = Camera.main.transform.position;
            pos.z = 0;
            Vector3 endPos = explainerQueue[0].lrEndPosGetter();
            endPos.z = 0;
            lr.SetPosition(0, pos + explainerQueue[0].lrStartPosition);
            lr.SetPosition(1, endPos);
            arrowHead.position = lr.GetPosition(1);
            arrowHead.up = (lr.GetPosition(1) - lr.GetPosition(0)).normalized * 5f;
        }
        else if (explainerQueue[0].uiHighlight)
        {
            eyeHighlight.gameObject.SetActive(true);
            SetHighlightPosition(explainerQueue[0].lrEndPosGetter());
            //eyeHighlight.position = explainerQueue[0].lrStartPosition;
        }

        explainerQueue[0].dialogueBox.SetActive(true);
        explainerQueue[0].dialogueBox.GetComponentInChildren<TextMeshProUGUI>().text = explainerQueue[0].text;

        if (PauseManager.instance != null)
            PauseManager.instance.megaPaused = true;
        Time.timeScale = 0;
    }
}
