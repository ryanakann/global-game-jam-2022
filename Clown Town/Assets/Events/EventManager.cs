using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EventTypes {
    ClownTalk,
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
    ClownIntro,
    ClownIntroMid,
    ClownIntroExit,
}

public class EventManager : Singleton<EventManager>
{
    [SerializeField]
    bool debug;
    [SerializeField]
    GameObject debugDialoguePrefab;
    [SerializeField]
    GameObject debugInterruptDialoguePrefab;

    float currentPriority;

    Stack<Dialogue> dialogueStack = new Stack<Dialogue>();

    private void Start()
    {
        if (debug)
        {
            GameObject debugDialogueObj = Instantiate(debugDialoguePrefab);
            Dialogue debugDialogue = debugDialogueObj.GetComponent<Dialogue>();
            PushDialogue(debugDialogue);
        }
    }

    public void PushDialogue(params Dialogue[] dialogues)
    {
        SelectionController.instance.ClearPanels();
        SelectionController.instance.canSelect = false;
        if (dialogueStack.Count > 0)
            dialogueStack.Peek().Pause();

        foreach (var dialogue in dialogues)
            dialogueStack.Push(dialogue);

        dialogueStack.Peek().Play();
        currentPriority = dialogueStack.Peek().priority;
    }


    public void FinishDialogue(Dialogue dialogueFinished)
    {
        Debug.Log("Finished Dialogue: " + dialogueFinished.Name);
        dialogueStack.Pop();
        if (dialogueStack.Count > 0)
        {
            dialogueStack.Peek().Play();
        }
        else
        {
            SelectionController.instance.canSelect = true;
        }
    }

    public void RaiseSignal()
    {
        // poll all the event listeners
        // signals have a default protocol for how to pick an event listener
        // they pick some responders, and get the Dialogues and push them on the stack 
    }
}
