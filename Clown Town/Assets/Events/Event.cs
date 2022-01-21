using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;

public delegate void EventReturn(Event _event);

public class Event : MonoBehaviour
{
    public string Name;

    [SerializeField]
    protected Flowchart flowchart;

    public EventReturn Return;

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

    public string getClownName(int queryId)
    {
        return ClownManager.getClownWithId(queryId).Name;
    }

    public void HarmClownByHealthFactor(int clownId, float healthFactor)
    {
        Clown schlimazel = ClownManager.getClownWithId(clownId);
        schlimazel.Harm(schlimazel.CurrentHealth * healthFactor);
    }

}
