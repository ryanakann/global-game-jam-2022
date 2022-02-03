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

    public void loadQuips(Dictionary<EventTypes, List<string>> quipDict)
    {

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

        foreach (KeyValuePair<EventTypes, string[]> quipsPair in eventsToQuips)
        {
            if (!quipDict.ContainsKey(quipsPair.Key))
                quipDict[quipsPair.Key] = new List<string>();
            
            foreach (string quip in quipsPair.Value)
            {
                quipDict[quipsPair.Key].Add(quip);
            }
        }

    }

}
