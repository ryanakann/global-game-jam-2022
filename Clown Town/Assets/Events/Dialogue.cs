using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;

public delegate void DialogueReturn(Dialogue _dialogue);

public class Dialogue : MonoBehaviour
{
    public string Name;

    [SerializeField]
    protected bool recruitEvent;

    [SerializeField]
    protected Flowchart flowchart;

    [SerializeField]
    ClownPersonality[] requiredPersonalities;

    [SerializeField]
    ClownTrait[] requiredTraits;

    public DialogueReturn Return;

    Block lastBlock;
    int lastCommandIndex;

    public void Begin()
    {
        if (!IsExecutable())
        {
            throw new System.Exception("Trying to Begin Dialogue without all trait or personality requirements being met");
        }
        flowchart.ExecuteBlock("Begin");
    }

    public void Finish()
    {
        Return?.Invoke(this);
    }

    public void TestInterrupt()
    {
        EventManager.TestDialogueInterrupt(this);
    }

    public bool IsExecutable()
    {
        bool executable = true;
        foreach (ClownPersonality personality in requiredPersonalities)
        {
            bool hasPersonality = ClownManager.HasClownWithPersonality(personality);
            if (!hasPersonality)
                print("ClownManager does not have personality " + personality);
            executable = executable && hasPersonality;
        }

        foreach (ClownTrait trait in requiredTraits)
        {
            bool hasTrait = ClownManager.HasClownWithTrait(trait);
            if (!hasTrait)
                print("ClownManager does not have trait " + trait);
            executable = executable && hasTrait;
        }

        return executable;
    }

    public void Pause()
    {
        lastBlock = flowchart.GetExecutingBlocks()[0];
        lastCommandIndex = lastBlock.ActiveCommand.CommandIndex;
        flowchart.StopAllBlocks();
    }

    public void Resume()
    {
        flowchart.ExecuteBlock(lastBlock, lastCommandIndex + 1);
    }

    /**
     * If a Clown with the given id exists, harm it by the given amount.
     * If no such Clown exists, throw an error.
     * 
     * Return whether or not the harmed Clown is still alive.
     */
    public bool harmClownWithId(int id, float harmAmount)
    {
        Clown clown = ClownManager.getClownWithId(id);
        clown.Harm(harmAmount);

        return clown.IsAlive();
    }

    public int getRandomClownId()
    {
        return ClownManager.getRandomClownId();
    }

    public int getRandomClownIdExcludingAnother(int excludeId)
    {
        return ClownManager.getRandomClownIdExcludingAnother(excludeId);
    }

    public int getClownIdWithTrait(ClownTrait queryTrait)
    {
        return ClownManager.getClownIdWithTrait(queryTrait);
    }

    public int getClownIdWithTraitExcludingAnother(int excludeId, ClownTrait queryTrait)
    {
        return ClownManager.getClownIdWithTraitExcludingAnother(excludeId, queryTrait);
    }

    public int GetClownIdWithPersonality(ClownPersonality queryPersonality)
    {
        return ClownManager.GetClownIdWithPersonality(queryPersonality);
    }

    public string getClownName(int queryId)
    {
        return ClownManager.getClownWithId(queryId).Name;
    }

    public void HarmClownByHealthFactor(int clownId, float healthFactor)
    {
        Clown schlimazel = ClownManager.getClownWithId(clownId);
        schlimazel.Harm(schlimazel.CurrentHealth * healthFactor);
    }

    public void RemoveClownTrait(int clownId, ClownTrait trait)
    {
        ClownManager.RemoveClownTrait(clownId, trait);
    }

    public void AddClownTrait(int clownId, ClownTrait trait)
    {
        ClownManager.AddClownTrait(clownId, trait);
    }

    public void GainPeanuts(int peanutsAmount)
    {
        print("You gain " + peanutsAmount + " peanuts");
        print("WE HAVE NOT IMPLEMENTED PEANUTS YOU FOOL");
    }

    public void GainEarwax(int waxAmount)
    {
        print("You gain " + waxAmount + " wax");
        print("WE HAVE NOT IMPLEMENTED WAX YOU FOOL");
    }

    public void LoseEarwax(int waxAmount)
    {
        print("You lose " + waxAmount + " wax");
        print("WE HAVE NOT IMPLEMENTED WAX YOU FOOL");
    }

    public void KillClown(int clownId)
    {
        ClownManager.KillClown(clownId);
    }

    public int GetCurrentWax()
    {
        print("WE HAVE NOT IMPLEMENTED WAX YOU FOOL");
        return 100;
    }

    public int GetCurrentPeanuts()
    {
        print("WE HAVE NOT IMPLEMENTED PEANUTS YOU FOOL");
        return 1000;
    }

    public string GetQuipForClownForEvent(int id, EventTypes eventType)
    {
        return ClownManager.GetQuipForClownForEvent(id, eventType);
    }

    public int GenerateClownWithPersonality(ClownPersonality personality)
    {
        int x = ClownManager.GenerateClownWithPersonality(personality);
        print(x);
        return x;
    }

}
