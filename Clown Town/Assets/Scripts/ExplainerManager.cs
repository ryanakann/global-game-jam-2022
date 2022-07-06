using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Explanation
{
    public string text;
    public GameObject dialogueBox;
    public Vector3 lrStartPosition, lrEndPosition;
    public bool usePointer;

    public Explanation(string text, GameObject dialogueBox, Vector3 lrStartPosition, Vector3 lrEndPosition, bool usePointer)
    {
        this.text = text;
        this.dialogueBox = dialogueBox;
        this.lrStartPosition = lrStartPosition;
        this.lrEndPosition = lrEndPosition;
        this.usePointer = usePointer;
    }
}

public class ExplainerManager : PersistentSingleton<ExplainerManager>
{

    Transform[] uiPivots = new Transform[9];
    Vector2[] lrPivots = new Vector2[9];
    LineRenderer lr;
    Transform arrowHead;


    GameObject blockerCanvas;

    List<Explanation> explainerQueue = new List<Explanation>();

    Dictionary<string, List<Explanation>> explainerMap;

    HashSet<string> explainedSet = new HashSet<string>();

    [HideInInspector]
    public List<ClownProfile> clowns = new List<ClownProfile>();

    bool explaining;
    [HideInInspector]
    public bool explain;

    protected override void Awake()
    {
        base.Awake();

        arrowHead = transform.FindDeepChild("arrow_head");

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
            new Dictionary<string, List<Explanation>>()
            {
                {"StartGame", new List<Explanation>()
                    { new Explanation("This is an example dialogue!", uiPivots[0].gameObject, lrPivots[0], new Vector3(3,4,0), true) }
                },
            };
    }

    private void Update()
    {
        if (explaining && Input.GetMouseButtonDown(0))
        {
            Nextplanation();
        }
    }

    public void Explain(string cue)
    {
        if (!explain)
            return;
        if (explainerMap.ContainsKey(cue) && !explainedSet.Contains(cue))
        {
            explainedSet.Add(cue);
            explainerQueue.AddRange(explainerMap[cue]);
        }

        if (!explaining)
        {
            if (FaderCanvas.instance.fading)
            {
                FaderCanvas.instance.finishedFading += Execute;
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
        explainerQueue.RemoveAt(0);
        if (explainerQueue.Count == 0)
        {
            explaining = false;
            blockerCanvas.SetActive(false);
            lr.enabled = false;
            arrowHead.gameObject.SetActive(false);
            Time.timeScale = 1;
            if (PauseManager.instance != null)
                PauseManager.instance.megaPaused = false;
            return;
        }
        Execute();
    }

    void Execute()
    {
        blockerCanvas.SetActive(true);
        FaderCanvas.instance.finishedFading -= Execute;

        explaining = true;

        lr.enabled = explainerQueue[0].usePointer;
        arrowHead.gameObject.SetActive(explainerQueue[0].usePointer);

        if (explainerQueue[0].usePointer)
        {
            lr.SetPosition(0, explainerQueue[0].lrStartPosition);
            lr.SetPosition(1, explainerQueue[0].lrEndPosition);
            arrowHead.position = lr.GetPosition(1);
            arrowHead.up = (lr.GetPosition(1) - lr.GetPosition(0)).normalized;
        }

        explainerQueue[0].dialogueBox.SetActive(true);
        explainerQueue[0].dialogueBox.GetComponentInChildren<TextMeshProUGUI>().text = explainerQueue[0].text;

        if (PauseManager.instance != null)
            PauseManager.instance.megaPaused = true;
        Time.timeScale = 0;
    }
}
