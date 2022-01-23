using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EventTypes {
    ClownHappy,
    ClownSad,
    ClownAngry,
    ClownStoic,
    ClownGetHurt,
    ClownGetKilled,
    AnotherClownHurt,
    AnotherClownKilled,
    ClownDying,
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
    [SerializeField]
    GameObject debugInterruptDialoguePrefab;

    static Dialogue interruptedDialogue;

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
            BeginDialogue(debugDialogue);
        }
    }

    void BeginDialogue(Dialogue dialogueToBegin)
    {
        dialogueToBegin.Return += FinishDialogue;
        dialogueToBegin.Begin();
    }

    void FinishDialogue(Dialogue dialogueFinished)
    {
        Debug.Log("Finished Dialogue: " + dialogueFinished.Name);
    }

    public static void TestDialogueInterrupt(Dialogue dialogueToInterrupt)
    {
        interruptedDialogue = dialogueToInterrupt;
        interruptedDialogue.Pause();
        interruptedDialogue.gameObject.SetActive(false);

        GameObject interruptingDialogueObj = Instantiate(instance.debugInterruptDialoguePrefab);
        Dialogue interruptingDialogue = interruptingDialogueObj.GetComponent<Dialogue>();
        interruptingDialogue.Return += FinishInterruptingDialogue;
        interruptingDialogue.Begin();
    }

    static void FinishInterruptingDialogue(Dialogue dialogueFinished)
    {
        Debug.Log("Finished Dialogue: " + dialogueFinished.Name);
        interruptedDialogue.gameObject.SetActive(true);
        interruptedDialogue.Resume();
    }
}
