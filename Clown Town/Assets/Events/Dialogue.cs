using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;

public delegate void DialogueReturn(Dialogue _dialogue);

public class Dialogue : MonoBehaviour
{
    public string Name;

    [SerializeField]
    protected Flowchart flowchart;

    [SerializeField]
    public ClownTrait[] requiredTraits;

    public DialogueReturn Return;

    public void Begin()
    {
        flowchart.ExecuteBlock("Begin");
    }

    public void Finish()
    {
        Return?.Invoke(this);
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

    public string GetClownResponseToEvent(int id, EventType eventType)
    {
        return "ugga";
    }

}
