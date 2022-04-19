using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[RequireComponent(typeof(CurrencyDropper))]
public class Enemy : Unit
{
    CurrencyDropper dropper;

    protected override void Start()
    {
        base.Start();
        dropper = GetComponent<CurrencyDropper>();
    }

    public override Unit SelectTarget()
    {
        Unit tempTarget = null;
        foreach (var ally in lane.allies)
        {
            if (ally == null)
                continue;
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

    public override void DieStart()
    {
        // drop shit
        dropper.Drop();
        base.DieStart();
    }
}
