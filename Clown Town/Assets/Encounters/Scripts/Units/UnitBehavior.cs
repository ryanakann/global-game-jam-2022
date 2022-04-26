using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Encounters
{
    public class UnitBehavior : MonoBehaviour, IInitializable<UnitInfo>
    {
        private UnitInfo _info;

        [HideInInspector]
        public UnityEvent<float> OnAttack;
        [HideInInspector]
        public UnityEvent<float> OnHealthChanged;
        [HideInInspector]
        public UnityEvent OnDie;

        public virtual void Init(UnitInfo unitInfo)
        {
            _info = unitInfo;

            _info.CurrentHealth = _info.MaxHealth;
        }

        // Update is called once per frame
        protected virtual void Update()
        {
            if (_info == null) return;
            if (_info.UnitType == UnitType.Foe) return;
            if (Input.GetKeyDown(KeyCode.Space))
            {
                TakeDamage(Random.Range(10f, 20f));
            }
            else if (Input.GetKeyDown(KeyCode.A))
            {
                Attack();
            }
        }

        protected virtual void Attack()
        {
            if (!_info.Alive) return;
            StartCoroutine(AttackCR());
        }

        protected virtual IEnumerator AttackCR()
        {
            print($"{gameObject.name} is attacking");
            var maxCoolddown = 1 / _info.AttackSpeed;
            
            OnAttack?.Invoke(_info.AttackSpeed);

            yield return new WaitForSeconds(1 / (2 * _info.AttackSpeed));

            if (_info.AttackEffect)
            {
                var effect = Instantiate(_info.AttackEffect, transform).GetComponent<UnitAttack>();
                effect.GetComponent<Animator>().speed = _info.AttackSpeed;

                effect.gameObject.SetActive(false);
                effect.Init(_info);
                effect.transform.localPosition = Vector3.right * _info.AttackRange / 2f; 
                effect.transform.localScale = new Vector3(_info.AttackRange, _info.AttackWidth, effect.transform.localScale.z);
                effect.gameObject.SetActive(true);
                Destroy(effect.gameObject, 1 / _info.AttackSpeed + 0.1f);
            }

            yield return new WaitForSeconds(_info.AttackCooldown);
        }

        public virtual void TakeDamage(float damage)
        {
            if (!_info.Alive) return;
            
            _info.CurrentHealth = Mathf.Max(0f, _info.CurrentHealth - damage);
            
            if (!_info.Alive)
            {
                Die();
            }

            OnHealthChanged?.Invoke(_info.CurrentHealth);
        }

        protected virtual void Die()
        {
            Debug.Log($"{name} is DEAD");
            OnDie?.Invoke();
            Destroy(gameObject, 1f);
        }
    }
}