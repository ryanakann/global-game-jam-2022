using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Encounters
{
    public class UnitBehavior_Ally : UnitBehavior
    {
        Transform targetPoint;
        float targetRadius = 0.1f;

        protected virtual void Start()
        {
            targetPoint = transform.FindDeepChild("target_point");
        }

        // Update is called once per frame

        protected override void Update()
        {
            base.Update();
            foreach (RaycastHit2D hit in Physics2D.CircleCastAll(targetPoint.position, targetRadius, Vector2.zero))
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
