using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Encounters
{
    public class UnitBehavior_Ally : UnitBehavior
    {
        Transform targetPoint;

        protected virtual void Start()
        {
            targetPoint = transform.FindDeepChild("target_point");
        }

        // Update is called once per frame

        protected override void Update()
        {
            base.Update();

            foreach (var hit in Physics2D.OverlapBoxAll(targetPoint.position, new Vector2(_info.AttackRange, _info.AttackWidth), 0f))
            {
                var unit = hit.transform.GetComponent<UnitBehavior>();
                if (unit != null && unit._info != null && unit._info.UnitType == UnitType.Foe)
                {
                    Attack();
                }
            }
        }
    }
}
