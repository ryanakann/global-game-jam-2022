using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ClownPersonality {
    Veteran,
    Cowboy,
    Mime,
    Chef,
    Honking,
    PhilosophicalOptimist,
    PhilosophicalPessimist,
    BadJokes,
    Hypochondriac,
    Bully
}

public enum ClownTrait {
    Angry,
    Happy,
    Sad,
    Clumsy,
}

[CreateAssetMenu(menuName = "CLOWN/ClownProfile", fileName = "new ClownProfile")]
public class ClownProfile : ScriptableObject
{
    public float health;
    public ClownPersonality personality;
    public List<ClownTrait> traits;

    public string[] getHurtDialogue;
    public string[] attackDialogue;
    public string[] gratefulDialogue;
}
