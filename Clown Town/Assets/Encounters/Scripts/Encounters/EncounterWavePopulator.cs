using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Encounters
{

    public class EncounterWavePopulator : MonoBehaviour
    {
        [Range(0f, 100f)]
        public float baseDifficulty = 1f;
        [Range(0f, 10f)]
        public float difficultyIncreasePerWave = 0f;
        [Range(-1, 999999)]
        public int seed;

        private int waves;

        private GameObject[] allies;
        private GameObject[] enemies;

        private void Awake()
        {
            waves = 0;
            if (seed >= 0)
            {
                Random.InitState(seed);
            }

            allies = Resources.LoadAll<GameObject>("Prefabs/Units/Allies");
            enemies = Resources.LoadAll<GameObject>("Prefabs/Units/Enemies");
        }

        public EncounterWave GetWave()
        {
            float difficulty = Mathf.Clamp(baseDifficulty + difficultyIncreasePerWave * waves, 0f, 100f);


            EncounterWave wave = new EncounterWave();
            wave.enemies = GetEnemyList(difficulty);
            wave.timeBeforeFirstSpawn = GetTimeBeforeFirstSpawn(difficulty);
            wave.timeBetweenEnemiesMean = GetTimeBetweenEnemiesMean(difficulty);
            wave.timeBetweenEnemiesVariance = GetTimeBetweenEnemiesVariance(difficulty);

            waves++;
            return wave;
        }

        // The objective of this function is to provide a sliding
        // probability distribution as difficulty increases. The
        // function goes from y = -x to y = x, centered about (0.5, 0.5).
        // The correct way to sample this distribution would have been
        // to integrate the equation and do some shenanigans to sample,
        // however that would take to much time to figure out, so I'm just
        // gonna create a hella large list and choose a random index ;)
        private List<UnitInfo> GetEnemyList(float difficulty)
        {

            enemies = Resources.LoadAll<GameObject>("Prefabs/Units/Enemies");
            if (enemies.Length == 0) return null;

            // Assumtions:
            // Difficulty goes from 0 to 100
            // Enemies are already ascending in order of difficulty
            float m = Mathf.Lerp(-1f, 1f, difficulty / 100f);
            List<int> indicesList = new List<int>();
            for (int i = 0; i < enemies.Length; i++)
            {
                float x = (float)(i + 0.5f) / enemies.Length;
                float y = m * x - 0.5f * m + 0.5f;

                // The higher the precision, the larger the list.
                // 2 should be fine for our needs.
                int precision = 2;
                float multiplier = Mathf.Pow(10f, precision);
                float count = (int)(y * multiplier);
                for (int j = 0; j < count; j++)
                {
                    indicesList.Add(i);
                }
                
            }


            List<UnitInfo> enemyList = new List<UnitInfo>();
            int enemyCount = Mathf.RoundToInt(Mathf.Lerp(5f, 20f, difficulty / 100f));
            for (int i = 0; i < enemyCount; i++)
            {
                var indexOfEnemyToAdd = indicesList[Random.Range(0, indicesList.Count)];
                var enemyToAdd = enemies[indexOfEnemyToAdd];
                enemyList.Add(enemyToAdd.GetComponent<UnitInfo>());
            }
            return enemyList;
        }

        private float GetTimeBeforeFirstSpawn(float difficulty)
        {
            return Mathf.Lerp(10f, 1f, difficulty / 100f);
        }

        private float GetTimeBetweenEnemiesMean(float difficulty)
        {
            return Mathf.Lerp(10f, 1f, difficulty / 100f);
        }

        private float GetTimeBetweenEnemiesVariance(float difficulty)
        {
            return 2f;
        }
    }
}
