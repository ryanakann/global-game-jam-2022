using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    [SerializeField]
    bool debug;
    [SerializeField]
    GameObject debugEventPrefab;

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
            GameObject debugEventObj = Instantiate(debugEventPrefab);
            Event debugEvent = debugEventObj.GetComponent<Event>();
            debugEvent.Begin();
        }
    }

    void BeginEvent(Event eventToBegin)
    {
        eventToBegin.Return += FinishEvent;
        eventToBegin.Begin();
    }

    void FinishEvent(Event eventFinished)
    {
        Debug.Log("Finished Event: " + eventFinished.Name);
    }

}
