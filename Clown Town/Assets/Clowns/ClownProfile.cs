using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ClownPersonality {
    Default,
    Veteran,
    Cowboy,
    Mime,
    Chef,
    Honking,
    PhilosophicalOptimist,
    PhilosophicalPessimist,
    BadJokes,
    Hypochondriac,
    Bully,
    Vociferous,
}

public enum ClownTrait {
    Angry,
    Happy,
    Sad,
    Clumsy,
    Stoic,
    Anxious,
}

[System.Serializable]
public class QuipTuple
{
    public EventTypes eventType;
    public string quips;
}

[CreateAssetMenu(menuName = "CLOWN/ClownProfile", fileName = "new ClownProfile")]
public class ClownProfile : ScriptableObject
{
    public float health;
    public ClownPersonality personality;
    public List<ClownTrait> traits;

    public string[] clickQuip;

    public string[] happyQuips;
    public string[] sadQuips;
    public string[] angryQuips;

    public string[] getHurtQuips;
    public string[] getKilledQuips;
    public string[] anotherHurtQuips;
    public string[] anotherKilledQuips;

    public List<QuipTuple> quipTuples = new List<QuipTuple>();



    public void loadQuips(Dictionary<EventTypes, List<string>> quipDict)
    {

        /*
        Dictionary<EventTypes, string[]> eventsToQuips = new Dictionary<EventTypes, string[]> {
            { EventTypes.ClownTalk, clickQuip },
            { EventTypes.ClownHappy, happyQuips },
            { EventTypes.ClownSad, sadQuips },
            { EventTypes.ClownAngry, angryQuips },
            { EventTypes.ClownGetHurt, getHurtQuips },
            { EventTypes.ClownGetKilled, getKilledQuips },
            { EventTypes.AnotherClownHurt, anotherHurtQuips },
            { EventTypes.AnotherClownKilled, anotherKilledQuips }
        };
        */

        foreach (var quipsPair in quipTuples)
        {
            if (!quipDict.ContainsKey(quipsPair.eventType))
                quipDict[quipsPair.eventType] = new List<string>();
            
            quipDict[quipsPair.eventType].Add(quipsPair.quips);
            quipDict[quipsPair.eventType].Shuffle();
        }

    }

}
