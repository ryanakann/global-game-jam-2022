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

        static public Vector3 GetSpawnerPos()
        {
            return instance.spawners[2].transform.position;
        }

        public void StartEncounter()
        {
            if (Active) return;
            Active = true;
            StartCoroutine(StartEncounterCR());
        }

        public void EndEncounter()
        {
            Active = false;
        }

        private IEnumerator StartEncounterCR()
        {

            while (Active)
            {
                var wave = gameObject.GetComponent<EncounterWavePopulator>().GetWave();

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