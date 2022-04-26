using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Encounters
{
    public class UnitTargeting : MonoBehaviour, IInitializable<UnitInfo>
    {
        private UnitInfo _info;

        [SerializeField]
        private TargetingMode _targetingMode;

        public void Init(UnitInfo unitInfo)
        {
            _info = unitInfo;
        }

        public UnitInfo GetTarget()
        {
            if (_info.Encounter == null) return null;

            List<UnitInfo> candidates = _info.UnitType == UnitType.Foe ?
                _info.Encounter.GetComponent<EncounterUnits>().allies :
                _info.Encounter.GetComponent<EncounterUnits>().enemies;


            UnitInfo target = null;
            float currentBest;

            switch (_targetingMode)
            {
                case TargetingMode.Closest:
                    currentBest = float.MaxValue;
                    break;
                case TargetingMode.Furthest:
                    currentBest = float.MinValue;
                    break;
                case TargetingMode.MaxHealth:
                    currentBest = float.MinValue;
                    break;
                case TargetingMode.MinHealth:
                    currentBest = float.MaxValue;
                    break;
                default:
                    currentBest = 0f;
                    break;
            }

            foreach (var unit in candidates)
            {
                float value = 0f;

                switch (_targetingMode)
                {
                    case TargetingMode.Closest:
                        value = unit.transform.position.x - transform.position.x;
                        if (value > currentBest) break;
                        target = unit;
                        currentBest = value;
                        break;
                    case TargetingMode.Furthest:
                        value = unit.transform.position.x - transform.position.x;
                        if (value < currentBest) break;
                        target = unit;
                        currentBest = value;
                        break;
                    case TargetingMode.MaxHealth:
                        value = unit.CurrentHealth;
                        if (value < currentBest) break;
                        target = unit;
                        currentBest = value;
                        break;
                    case TargetingMode.MinHealth:
                        value = unit.CurrentHealth;
                        if (value > currentBest) break;
                        target = unit;
                        currentBest = value;
                        break;
                    default:
                        break;
                }
            }
            return target;
        }

        public enum TargetingMode
        {
            Closest,
            Furthest,
            MaxHealth,
            MinHealth,
        }
    }
}