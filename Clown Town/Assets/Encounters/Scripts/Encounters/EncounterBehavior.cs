using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Encounters
{
    public class EncounterBehavior : MonoBehaviour, IInitializable<EncounterInfo>
    {
        private EncounterInfo _info;

        public bool Active { get; private set; }

        public void Init(EncounterInfo info)
        {
            _info = info;

            Active = false;
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
                    var instance = Instantiate(enemy, transform);
                    Vector2 point = _info.GridToWorldPosition(new Vector2(_info.gridDimensions.x, Random.Range(0, _info.gridDimensions.y)));
                    instance.transform.position = point;
                    yield return new WaitForSeconds(wave.timeBetweenEnemiesMean + Random.Range(-wave.timeBetweenEnemiesVariance / 2f, wave.timeBetweenEnemiesVariance / 2f));
                }
            }
        }
    }
}