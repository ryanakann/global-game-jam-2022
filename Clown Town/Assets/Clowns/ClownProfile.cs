using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ClownTrait {
    Angry,
    Happy,
    Sad,
    Veteran,
    Clumsy,
}

[CreateAssetMenu(menuName = "CLOWN/ClownProfile", fileName = "new ClownProfile")]
public class ClownProfile : ScriptableObject
{
    public float health;
    public List<ClownTrait> traits;
}
