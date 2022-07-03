using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class IntroDialogueManager : Singleton<IntroDialogueManager>
{

    List<Clown> clownQueue = new List<Clown>();
    List<string> lineQueue = new List<string>();
    Dictionary<ClownPersonality, Dictionary<EventTypes, int>> lineCounter = new Dictionary<ClownPersonality, Dictionary<EventTypes, int>>();
    Transform clownHolder, startClownPoint;

    ClownDisplay display;

    Animator anim;

    public bool ready;


    // Start is called before the first frame update
    void Start()
    {
        clownHolder = transform.FindDeepChild("IntroClownHolder");
        startClownPoint = transform.FindDeepChild("ClownHolderStart");
        anim = GetComponent<Animator>();
        // get the clowns
        clownQueue = ClownManager.GetClowns();
        Play();
        // queue em up

        ExplainerManager.instance.Explain("StartGame");
    }

    // Update is called once per frame
    void Update()
    {
        if (ClownManager.instance.quipFlowchart.GetExecutingBlocks().Count == 0 && ready)
        {
            if (lineQueue.Count > 0)
            {
                ClownManager.SayLineInFlowchartForClown(clownQueue[0].Id, lineQueue[0]);
                lineQueue.RemoveAt(0);
            }
            else if (lineQueue.Count == 0)
            {
                anim.SetTrigger("Exit");
            }
        }
    }

    void PushQuips(ClownPersonality trait, EventTypes eventType, int num = 1)
    {
        if (!lineCounter.ContainsKey(trait))
        {
            lineCounter[trait] = new Dictionary<EventTypes, int>() { { eventType, 0 } };
        }
        else if (!lineCounter[trait].ContainsKey(eventType))
        {
            lineCounter[trait][eventType] = 0;
        }

        if (lineCounter[trait][eventType] + num > ClownManager.eventQuips[trait][eventType].Count)
        {
            ClownManager.eventQuips[trait][eventType].Shuffle();
            lineCounter[trait][eventType] = 0;
        }
        if (num > ClownManager.eventQuips[trait][eventType].Count)
        {
            num = ClownManager.eventQuips[trait][eventType].Count;
        }

        for (int i = 0; i < num; i++)
        {
            lineQueue.Add(ClownManager.eventQuips[trait][eventType][lineCounter[trait][eventType]]);
            lineCounter[trait][eventType]++;
        }
    }

    void Play()
    {
        clownHolder.position = startClownPoint.position;
        display = clownQueue[0].SpawnDisplayAtPosition(clownHolder.position);
        display.transform.parent = clownHolder;
        display.transform.position += new Vector3(0, 0.5f, 0);
        display.SetVisibility(SpriteMaskInteraction.None);
        // get clown type, event type
        // if counter contains clown type and event type and the value equals the number of quips, delete that event type entry
        if (!ClownManager.eventQuips[clownQueue[0].Personality].ContainsKey(EventTypes.ClownIntro))
        {
            lineQueue.Add("Placeholder line~");
            lineQueue.Add("Goodbye");
            anim.SetTrigger("Play");
            return;
        }
        PushQuips(clownQueue[0].Personality, EventTypes.ClownIntro);
        PushQuips(clownQueue[0].Personality, EventTypes.ClownIntroMid, Random.Range(0, 3));
        PushQuips(clownQueue[0].Personality, EventTypes.ClownIntroExit);
        anim.SetTrigger("Play");
    }

    public void NextClown()
    {
        display.SetVisibility(SpriteMaskInteraction.VisibleInsideMask);
        clownQueue.RemoveAt(0);
        ready = false;
        if (clownQueue.Count == 0)
        {
            FaderCanvas.instance.GoAway("LevelSelect");
        }
        else
        {
            Play();
        }
    }
}
