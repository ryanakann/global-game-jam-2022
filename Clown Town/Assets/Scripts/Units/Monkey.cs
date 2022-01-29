using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Monkey : Unit
{
    public override Unit SelectTarget()
    {
        base.SelectTarget();

        var target = lane.enemies.OrderBy(unit => Vector3.Distance(unit.transform.position, transform.position)).FirstOrDefault();
        return target;
    }

    protected override void IdleUpdate()
    {
        base.IdleUpdate();

        var target = lane.enemies.OrderBy(unit => Vector3.Distance(unit.transform.position, transform.position)).FirstOrDefault();
        if (target != null) return;

        this.target = target;
        state = State.Attack;
    }

    protected override void AttackUpdate()
    {
        base.AttackUpdate();


    }
}
