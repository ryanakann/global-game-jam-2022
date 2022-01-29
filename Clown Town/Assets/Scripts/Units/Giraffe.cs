using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Giraffe : Unit
{
    public override Unit SelectTarget()
    {
        Unit tempTarget = null;
        float minDist = float.MaxValue;
        foreach (var enemy in lane.enemies)
        {
            if (enemy.transform.position.x > Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0f)).x) continue;
            if (enemy.transform.position.x < transform.position.x) continue;
            float distance = enemy.transform.position.x - transform.position.x;
            if (distance < minDist)
            {
                tempTarget = enemy;
                minDist = distance;
            }
        }
        target = tempTarget;
        return target;
    }

    protected override void IdleUpdate()
    {
        base.IdleUpdate();
        if (SelectTarget() == null) return;
        AttackStart();
    }

    protected override void AttackUpdate()
    {
        base.AttackUpdate();

        if (SelectTarget() == null) IdleStart();
    }

    protected override void AttackExecute()
    {
        base.AttackExecute();

        Debug.Log($"{name} attacked {target.name}!");
        target.TakeDamage(attackDamage);
    }
}
