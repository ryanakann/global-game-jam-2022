using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Encounters
{
    public class EncounterInput : MonoBehaviour, IInitializable<EncounterInfo>
    {
        private EncounterInfo _info;

        private InputState _state;

        public void Init(EncounterInfo info)
        {
            _info = info;
        }

        private void Update()
        {
            if (Input.GetMouseButtonUp(0))
            {
                if (_state == InputState.None)
                {
                    // Noop right now
                }
                else if (_state == InputState.PlacingUnit)
                {
                    var primedUnit = _info.GetComponent<EncounterContext>().primedUnit;
                    if (primedUnit == null)
                    {
                        _state = InputState.None;
                        return;
                    }

                    var mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    if (_info.IsPointWithinGrid(mousePosition))
                    {
                        _info.GetComponent<EncounterUnits>().AddAllyUnit(primedUnit, false, _info.WorldToGridPosition(mousePosition));
                    }
                    SetState(InputState.None);
                }
            }
            else if (Input.GetMouseButtonUp(1))
            {
                SetState(InputState.None);
            }
        }

        public void SetState(InputState state)
        {
            _state = state;
        }
    }

    public enum InputState
    {
        PlacingUnit,
        None
    }
}