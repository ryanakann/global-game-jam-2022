using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Encounters
{
    public class EncounterBehavior : Singleton<EncounterBehavior>, IInitializable<EncounterInfo>
    {
        [HideInInspector]
        public EncounterInfo _info;

        [HideInInspector]
        public EncounterUnits encounterUnits;

        //Dictionary<Vector2, Animator>
        List<EnemySpawner> spawners = new List<EnemySpawner>();

        public bool Active { get; private set; }

        public void Init(EncounterInfo info)
        {
            _info = info;
            encounterUnits = GetComponent<EncounterUnits>();

            Active = false;

            foreach (EnemySpawner spawner in GetComponentsInChildren<EnemySpawner>())
            {
                spawner.enemyParent = transform;
                spawner._info = _info;
                spawners.Add(spawner);
            }

            StartEncounter();
        }

        public void StartEncounter()
        {
            if (Active) return;
            Active = true;
            StartCoroutine(StartEncounterCR());
        }

        private IEnumerator StartEncounterCR()
        {
            var waves = _info.waves;

            foreach (var wave in waves)
            {
                yield return new WaitForSeconds(wave.timeBeforeFirstSpawn);

                foreach (var enemy in wave.enemies)
                {
                    spawners[Random.Range(0, spawners.Count)].AddEnemy(enemy);
                    yield return new WaitForSeconds(wave.timeBetweenEnemiesMean + Random.Range(-wave.timeBetweenEnemiesVariance / 2f, wave.timeBetweenEnemiesVariance / 2f));
                }
            }
        }
    }
}