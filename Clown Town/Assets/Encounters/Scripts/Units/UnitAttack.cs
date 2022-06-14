using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Encounters
{
    public class UnitAttack : MonoBehaviour, IInitializable<UnitInfo>
    {
        UnitInfo _info;

        public void Init(UnitInfo info)
        {
            _info = info;

            GetComponentInChildren<Animator>().speed = _info.AttackSpeed;

            gameObject.SetActive(false);
            transform.localPosition = Vector3.right * _info.AttackRange / 2f;
            transform.localScale = new Vector3(_info.AttackRange, _info.AttackWidth, transform.localScale.z);
            gameObject.SetActive(true);
            Destroy(gameObject, 1 / _info.AttackSpeed + 0.1f);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (_info == null) return;
            var enemyInfo = collision.GetComponent<UnitInfo>();
            if (enemyInfo && enemyInfo.UnitType != _info.UnitType)
            {
                enemyInfo.gameObject.GetComponent<UnitBehavior>().TakeDamage(_info.AttackDamage);
            }
        }
    }
}