using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "New Level", menuName = "ScriptableObjects/Level")]
public class Level : ScriptableObject
{
    public const float levelWidth = 9f;
    public const float levelHeight = 8f;
    public Rect levelArea { get => new Rect(0.5f - levelWidth / 2f, 0f - levelHeight / 2f, levelWidth, levelHeight); }

    public Scene scene;
    public LevelDetails details;

    public int laneCount = 7;

    public List<Lane> lanes;

    public void GenerateLanes()
    {
        lanes = new List<Lane>();
        for (int i = 0; i < laneCount; i++)
        {
            lanes.Add(new Lane(i, laneCount, levelArea));
        }
    }
}
