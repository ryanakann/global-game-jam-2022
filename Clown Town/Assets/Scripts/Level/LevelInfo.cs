using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "New Level", menuName = "ScriptableObjects/Level")]
public class LevelInfo : ScriptableObject
{
    [Range(1, 16)]
    public int laneCount;
    [Range(1, 16)]
    public int cellCountPerLane;

    public Scene scene;
    public LevelSkin details;

    public float initialWaitTime = 20f;
    public float waveDuration;
    public AnimationCurve waveCurve;

    public List<GameObject> enemies;
}
