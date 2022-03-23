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
                var effect = Instantiate(_info.AttackEffect, transform);
                effect.GetComponent<Animator>().speed = _info.AttackSpeed;
                effect.transform.localPosition = Vector3.right; 
                effect.transform.localScale = new Vector3(effect.transform.localScale.x, _info.AttackWidth, effect.transform.localScale.z);
                Destroy(effect, 1f);
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