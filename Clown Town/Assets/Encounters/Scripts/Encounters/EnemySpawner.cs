using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Encounters;

public class EnemySpawner : MonoBehaviour
{
    List<UnitInfo> queue = new List<UnitInfo>();
    [HideInInspector]
    public Transform enemyParent;

    Animator anim;
    [HideInInspector]
    public EncounterInfo _info;


    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void AddEnemy(UnitInfo enemy)
    {
        queue.Add(enemy);
        anim.SetBool("Spawn", true);
    }

    public void SpawnEnemy()
    {
        if (queue.Count > 0)
        {
            var enemy = queue[0];
            var instance = Instantiate(enemy, enemyParent);
            instance.MaxHealth *= EncounterWavePopulator.instance.difficultyMultiplier;
            instance.CurrentHealth *= EncounterWavePopulator.instance.difficultyMultiplier;
            instance.AttackSpeed *= EncounterWavePopulator.instance.difficultyMultiplier;
            instance.AttackDamage *= EncounterWavePopulator.instance.difficultyMultiplier;
            Vector2 point = _info.WorldToGridPosition(transform.position);
            instance.transform.position = _info.GridToWorldPosition(point);
            queue.RemoveAt(0);
            if (queue.Count == 0)
                anim.SetBool("Spawn", false);
        }
    }
}
