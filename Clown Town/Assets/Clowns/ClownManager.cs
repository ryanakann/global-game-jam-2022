using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClownManager : MonoBehaviour
{
    [SerializeField]
    bool debug;
    [SerializeField]
    List<ClownProfile> clownProfiles;

    [SerializeField]
    string[] clownFirstNames;
    [SerializeField]
    string[] clownLastNames;

    public static ClownManager instance;

    private static Dictionary<int, Clown> clowns = new Dictionary<int, Clown>();
    private static Dictionary<ClownTrait, List<Clown>> clownsByTrait = new Dictionary<ClownTrait, List<Clown>>();
    private static Dictionary<ClownPersonality, List<Clown>> clownsByPersonality = new Dictionary<ClownPersonality, List<Clown>>();

    private static Dictionary<ClownPersonality, Dictionary<EventTypes, List<string>>> eventQuips = new Dictionary<ClownPersonality, Dictionary<EventTypes, List<string>>>();

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        if (debug)
        {
            foreach (ClownProfile profile in clownProfiles)
            {
                Clown newClown = new Clown(profile);
                addClown(newClown);

                if (!eventQuips.ContainsKey(profile.personality))
                    eventQuips[profile.personality] = new Dictionary<EventTypes, List<string>>();
                profile.loadQuips(eventQuips[profile.personality]);
            }
        }
    }

    public static void addClown(Clown newClown)
    {
        clowns.Add(newClown.Id, newClown);
        foreach (ClownTrait trait in newClown.Traits)
        {
            if (!clownsByTrait.ContainsKey(trait))
            {
                clownsByTrait[trait] = new List<Clown>();
            }
            clownsByTrait[trait].Add(newClown);

        }

        if (!clownsByPersonality.ContainsKey(newClown.Personality))
        {
            clownsByPersonality[newClown.Personality] = new List<Clown>();
        }
        clownsByPersonality[newClown.Personality].Add(newClown);
    }

    public static bool HasClownWithPersonality(ClownPersonality personality)
    {
        return clownsByPersonality.ContainsKey(personality) && clownsByPersonality[personality].Count > 0;
    }

    public static bool HasClownWithTrait(ClownTrait trait)
    {
        return clownsByTrait.ContainsKey(trait) & clownsByTrait[trait].Count > 0;
    }

    public static Clown getClownWithId(int id)
    {
        if (!clowns.ContainsKey(id))
        {
            throw new System.Exception("No Clown exists with id " + id);
        }        
        return clowns[id];
    }

    public static int getRandomClownId()
    {
        return clowns.Keys.ElementAt(Random.Range(0, clowns.Count));
    }

    public static int getRandomClownIdExcludingAnother(int excludeId)
    {
        if (clowns.Count <= 1)
        {
            throw new System.Exception("Not enough Clowns to sample and exclude.");
        }

        int foundId = excludeId;
        while (foundId == excludeId)
        {
            foundId = getRandomClownId();
        }
        return foundId;
    }

    public static int GetClownIdWithPersonality(ClownPersonality queryPersonality)
    {
        if (clownsByPersonality.ContainsKey(queryPersonality) && clownsByPersonality[queryPersonality].Count > 0)
        {
            return clownsByPersonality[queryPersonality].ElementAt(Random.Range(0, clownsByPersonality[queryPersonality].Count)).Id;
        }
        else
        {
            throw new System.Exception("No Clowns with the trait " + queryPersonality);
        }
    }

    public static int getClownIdWithTrait(ClownTrait queryTrait)
    {
        if (clownsByTrait.ContainsKey(queryTrait) && clownsByTrait[queryTrait].Count > 0)
        {
            return clownsByTrait[queryTrait].ElementAt(Random.Range(0, clownsByTrait[queryTrait].Count)).Id;
        }
        else
        {
            throw new System.Exception("No Clowns with the trait " + queryTrait);
        }
        
    }

    public static int getClownIdWithTraitExcludingAnother(int excludeId, ClownTrait queryTrait)
    {
        if (clowns.Count <= 1 || (clownsByTrait[queryTrait].Count == 1 && clownsByTrait[queryTrait].ElementAt(0).Id == excludeId))
        {
            throw new System.Exception("Not enough Clowns to sample and exclude.");
        }

        int foundId = excludeId;
        while (foundId == excludeId)
        {
            if (clownsByTrait.ContainsKey(queryTrait) && clownsByTrait[queryTrait].Count > 0)
            {
                foundId = clownsByTrait[queryTrait].ElementAt(Random.Range(0, clownsByTrait[queryTrait].Count)).Id;
            }
            else
            {
                throw new System.Exception("No Clowns with the trait " + queryTrait);
            }
        }

        return foundId;
    }

    public static string getClownName()
    {
        string firstName = instance.clownFirstNames[Random.Range(0, instance.clownFirstNames.Length)];
        string lastName = instance.clownLastNames[Random.Range(0, instance.clownLastNames.Length)];
        return firstName + " " + lastName;
    }

    public static void KillClown(int clownId)
    {
        if (clowns.ContainsKey(clownId))
        {
            Clown theClown = clowns[clownId];
            foreach (ClownTrait trait in theClown.Traits)
            {
                clownsByTrait[trait].Remove(theClown);
            }
            theClown.Kill();
            clowns.Remove(clownId);
        }
    }

    public static void RemoveClownTrait(int clownId, ClownTrait trait)
    {
        if (clownsByTrait[trait].Contains(getClownWithId(clownId))) {
            clownsByTrait[trait].Remove(getClownWithId(clownId));
        }
    }

    public static void AddClownTrait(int clownId, ClownTrait trait)
    {
        if (!clownsByTrait[trait].Contains(getClownWithId(clownId)))
        {
            clownsByTrait[trait].Add(getClownWithId(clownId));
        }
    }

    public static string GetQuipForClownForEvent(int clownId, EventTypes eventType)
    {
        Clown clown = getClownWithId(clownId);
        ClownPersonality personality = clown.Personality;

        if (eventQuips.ContainsKey(personality))
        {
            Dictionary<EventTypes, List<string>> personalityQuips = eventQuips[personality];
            if (personalityQuips.ContainsKey(eventType))
            {
                List<string> quipsList = personalityQuips[eventType];
                if (quipsList.Count > 0)
                {
                    return quipsList[Random.Range(0, quipsList.Count)];
                }
            }
        }
        
        if (eventQuips.ContainsKey(ClownPersonality.Default))
        {
            Dictionary<EventTypes, List<string>> personalityQuips = eventQuips[ClownPersonality.Default];
            if (personalityQuips.ContainsKey(eventType))
            {
                List<string> quipsList = personalityQuips[eventType];
                if (quipsList.Count > 0)
                {
                    return quipsList[Random.Range(0, quipsList.Count)];
                }
            }
        }

        return "*mumbles incoherently*";
    }
}
