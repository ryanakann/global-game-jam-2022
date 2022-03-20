using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Encounters
{
    public class EncounterUnits : MonoBehaviour, IInitializable<EncounterInfo>
    {
        private EncounterInfo _info;

        public void Init(EncounterInfo info)
        {
            _info = info;
        }

        public void AddUnit(UnitInfo unitInfo, bool isInstance, Vector2 gridPosition, UnitType type)
        {
            var unit = isInstance ? unitInfo.gameObject : Instantiate(unitInfo.gameObject);
            unitInfo = unit.GetComponent<UnitInfo>();

            unitInfo.Init();

            unitInfo.UnitType = type;
            unitInfo.SetGridPosition(_info, gridPosition);
        }
    }
}