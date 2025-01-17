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

    public List<Horde> wave;

    [System.Serializable]
    public class Horde
    {
        public float waitTime = 10f;
        public List<GameObject> enemies;
    }
}
