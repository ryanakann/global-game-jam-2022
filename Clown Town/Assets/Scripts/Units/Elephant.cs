using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Elephant : Unit
{
    public List<Unit> targets;

    public List<Unit> SelectTargets()
    {
        List<Unit> targets = new List<Unit>();

        int laneIndex = lane.index;
        List<Lane> lanes = new List<Lane>();
        lanes.Add(LevelManager.instance.lanes[laneIndex]);
        if (laneIndex > 0) lanes.Add(LevelManager.instance.lanes[laneIndex - 1]);
        if (laneIndex < lanes.Count - 1) lanes.Add(LevelManager.instance.lanes[laneIndex + 1]);

        foreach (var lane in lanes)
        {
            Unit target = null;
            float minDist = float.MaxValue;
            foreach (var enemy in lane.enemies)
            {
                if (enemy.transform.position.x > Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0f)).x) continue;
                if (enemy.transform.position.x < transform.position.x) continue;
                float distance = enemy.transform.position.x - transform.position.x;
                if (distance < minDist)
                {
                    target = enemy;
                    minDist = distance;
                }
                if (target != null) targets.Add(target);
            }
        }
        this.targets = targets;
        return targets;
    }

    protected override void IdleUpdate()
    {
        base.IdleUpdate();
        if (SelectTargets() == null) return;
        AttackStart();
    }

    protected override void AttackUpdate()
    {
        base.AttackUpdate();

        if (SelectTargets() == null) IdleStart();
    }

    protected override void AttackExecute()
    {
        base.AttackExecute();

        targets.ForEach(target => {
            if (target == null) return;
            Debug.Log($"{name} attacked {target.name}!");
            target.TakeDamage(attackDamage);
            target.transform.position += Vector3.right * 0.5f;
        });
    }
}
