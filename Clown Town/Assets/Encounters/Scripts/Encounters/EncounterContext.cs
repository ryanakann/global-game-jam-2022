using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Encounters
{
    public class EncounterContext : MonoBehaviour, IInitializable<EncounterInfo>
    {
        private EncounterInfo _info;

        public UnitInfo primedUnit;

        public void Init(EncounterInfo info)
        {
            _info = info;
        }

    }
}