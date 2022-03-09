using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Encounters
{
    public class UnitUI : MonoBehaviour, IInitializable<UnitInfo>
    {
        private UnitInfo _unitInfo;

        [SerializeField]
        private Canvas _canvas;
        [SerializeField]
        private RawImage _redBar;
        [SerializeField]
        private RawImage _orangeBar;
        [SerializeField]
        private RawImage _greenBar;

        public void Init(UnitInfo unitInfo)
        {
            _unitInfo = unitInfo;

            _unitInfo.GetComponent<UnitBehavior>().OnHealthChanged?.AddListener(UpdateHealth);
        }

        public void SetHealth(float health)
        {
            _greenBar.rectTransform.localScale = new Vector3(health / _unitInfo.MaxHealth, 1f, 1f);
            _orangeBar.rectTransform.localScale = new Vector3(health / _unitInfo.MaxHealth, 1f, 1f);
        }

        private bool _hitInProgress = false;
        private float _targetPercentage = 0f;
        private const float _hitDuration = 2f;

        public void UpdateHealth(float health)
        {
            _targetPercentage = health / _unitInfo.MaxHealth;
            _greenBar.rectTransform.localScale = new Vector3(_targetPercentage, 1f, 1f);

            if (!_hitInProgress)
            {
                _hitInProgress = true;
                StartCoroutine(UpdateHealthCR());
            }
        }

        private IEnumerator UpdateHealthCR() 
        {
            yield return new WaitForSeconds(0.2f);   
            float currentPercentage = _orangeBar.rectTransform.localScale.x;
            while (currentPercentage > _targetPercentage)
            {
                _orangeBar.rectTransform.localScale = new Vector3(currentPercentage, 1f, 1f);
                currentPercentage -= Time.deltaTime / _hitDuration;
                yield return new WaitForEndOfFrame();
            }
            _orangeBar.rectTransform.localScale = new Vector3(_targetPercentage, 1f, 1f);
            _hitInProgress = false;
        }
    }
}