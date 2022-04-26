using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Encounters
{
    [System.Serializable]
    public class EncounterWave
    {
        public List<UnitInfo> enemies;
        [Range(0f, 30f)]
        public float timeBeforeFirstSpawn = 10f;
        [Range(0f, 10f)]
        public float timeBetweenEnemiesMean = 2f;
        [Range(0f, 10f)]
        public float timeBetweenEnemiesVariance = 2f;
        
    }
}