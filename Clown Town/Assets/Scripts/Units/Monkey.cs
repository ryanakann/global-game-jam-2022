using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Monkey : Unit
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

        if (projectile != null)
        {
            GameObject poop = Instantiate(projectile);
            poop.transform.parent = transform; 
            if (target != null)
                poop.GetComponent<Projectile>().Throw(transform.position, true, target.transform, attackDamage);
        }
    }
}
