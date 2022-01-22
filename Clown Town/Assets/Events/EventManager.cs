using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EventTypes {
    ClownHappy,
    ClownSad,
    ClownHurt,
    ClownDying,
    AnotherClownDied,
    LevelFinish,
    InTransit,
    CarStop,
    MusicClose,
}

public class EventManager : MonoBehaviour
{
    [SerializeField]
    bool debug;
    [SerializeField]
    GameObject debugDialoguePrefab;

    public static EventManager instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        if (debug)
        {
            GameObject debugDialogueObj = Instantiate(debugDialoguePrefab);
            Dialogue debugDialogue = debugDialogueObj.GetComponent<Dialogue>();
            debugDialogue.Begin();
        }
    }

    void BeginDialogue(Dialogue dialogueToBegin)
    {
        dialogueToBegin.Return += FinishEvent;
        dialogueToBegin.Begin();
    }

    void FinishEvent(Dialogue dialogueFinished)
    {
        Debug.Log("Finished Dialogue: " + dialogueFinished.Name);
    }

}
