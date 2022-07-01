using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explanation
{
    public string text;
    public Vector3 position;
    public Vector3 pointerPosition;
    public bool usePointer;

    public Explanation(string text, Vector3 position, Vector3 pointerPosition, bool usePointer)
    {
        this.text = text;
        this.position = position;
        this.pointerPosition = pointerPosition;
        this.usePointer = usePointer;
    }
}

public class ExplainerManager : PersistentSingleton<ExplainerManager>
{
    public GameObject explainerPrefab;

    Transform
        topLeft, topMid, topRight,
        centerLeft, centerMid, centerRight,
        bottomLeft, bottomMid, bottomRight;

    GameObject blockerCanvas;

    List<Explanation> explainerQueue = new List<Explanation>();

    Dictionary<string, List<Explanation>> explainerMap = new Dictionary<string, List<Explanation>>()
    { 

    };

    HashSet<string> explainedSet = new HashSet<string>();

    bool explaining;

    // GameObject currentExplainer;

    [HideInInspector]
    public bool explain;

    private void Start()
    {
        // initialize pivots
    }

    public void Explain(string cue)
    {
        if (!explain)
            return;
        if (explainerMap.ContainsKey(cue) && !explainedSet.Contains(cue))
        {
            explainedSet.Add(cue);
            // spawn those cues
            explainerQueue.AddRange(explainerMap[cue]);
        }

        if (!explaining)
        {
            blockerCanvas.SetActive(true);
            PauseManager.instance.megaPaused = true;
            Instantiate(explainerPrefab).GetComponent<Explainer>().SetExplanation(explainerQueue[0]); // give it the explanation?
            PauseManager.instance.PauseTime(0);
        }
    }

    public void ResetExplainer()
    {
        explainedSet.Clear();
        explain = false;
        explaining = false;
        if (PauseManager.instance != null)
            PauseManager.instance.megaPaused = false;
    }

    public void Nextplanation()
    {
        explainerQueue.RemoveAt(0);
        if (explainerQueue.Count == 0)
        {
            blockerCanvas.SetActive(false);
            PauseManager.instance.PauseTime(1);
            return;
        }
        Instantiate(explainerPrefab).GetComponent<Explainer>().SetExplanation(explainerQueue[0]);
    }
}
