using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Enemy : Unit
{
    public override Unit SelectTarget()
    {
        Unit tempTarget = null;
        foreach (var ally in lane.allies)
        {
            if (ally.transform.position.x > transform.position.x) continue;
            float distance = transform.position.x - ally.transform.position.x;
            if (distance < attackRange)
            {
                tempTarget = ally;
                break;
            }
        }
        target = tempTarget;
        return target;
    }

    protected override void IdleUpdate()
    {
        base.IdleUpdate();
        MoveStart();
    }

    protected override void MoveUpdate()
    {
        // Behavior
        base.MoveUpdate();
        transform.position += Vector3.left * speed * Time.deltaTime;

        // Transitions
        if (SelectTarget() != null) AttackStart();
    }

    protected override void AttackUpdate()
    {
        // Behavior
        base.AttackUpdate();

        // Transition
        if (SelectTarget() == null) MoveStart();
    }

    protected override void AttackExecute()
    {
        base.AttackExecute();
        target.TakeDamage(attackDamage);
    }
}
