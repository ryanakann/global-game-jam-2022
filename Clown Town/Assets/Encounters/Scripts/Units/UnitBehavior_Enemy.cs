using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Encounters
{
    [RequireComponent(typeof(CurrencyDropper))]
    public class UnitBehavior_Enemy : UnitBehavior
    {
        CurrencyDropper dropper;
        Transform targetPoint;
        float targetRadius = 0.1f;

        Vector3 speed = new Vector3(-1f, 0, 0);

        protected virtual void Start()
        {
            dropper = GetComponent<CurrencyDropper>();
            targetPoint = transform.FindDeepChild("target_point");
            //flip = true;
        }

        protected override void Update()
        {
            base.Update();
            bool move = true;
            foreach (RaycastHit2D hit in Physics2D.CircleCastAll(targetPoint.position, targetRadius, Vector2.zero))
            {
                var unit = hit.transform.GetComponent<UnitBehavior>();
                if (unit != null && unit._info != null && unit._info.UnitType == UnitType.Friend)
                {
                    move = false;
                    Attack();
                }
            }
            if (!move)
                return;
            transform.Translate(speed * Time.deltaTime);
        }

        protected override void Die()
        {
            dropper.Drop();
            base.Die();
        }
    }
}
