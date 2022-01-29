using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "CLOWN/LocationType", fileName = "new LocationType")]
public class LocationType : ScriptableObject
{
    public float difficultyMin;
    public float difficultyMax;

    public string[] locationNames;
    public string[] locationDescriptions;

    public GameObject[] eventPrefabs;
    public Sprite[] locationImages;
}
