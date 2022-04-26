using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Encounters
{
    public class EncounterDebug : MonoBehaviour, IInitializable<EncounterInfo>
    {
        private EncounterInfo _info;
        public new bool enabled;

        public UnitInfo testUnit;

        public void Init(EncounterInfo info)
        {
            _info = info;

            if (!Application.isPlaying) return;

            foreach (var unit in FindObjectsOfType<UnitInfo>())
            {
                if (unit.UnitType == UnitType.Foe) continue;
                int x = Random.Range(0, _info.gridDimensions.x);
                int y = Random.Range(0, _info.gridDimensions.y);
                _info.GetComponent<EncounterUnits>().AddAllyUnit(unit, true, new Vector2(x, y));
            }

            for (int i = 0; i < 1; i++)
            {
                int x = Random.Range(0, _info.gridDimensions.x);
                int y = Random.Range(0, _info.gridDimensions.y);
                _info.GetComponent<EncounterUnits>().AddAllyUnit(testUnit, false, new Vector2(x, y));
            }
        }

        private void OnDrawGizmos()
        {
            if (!enabled) return;
            if (!_info)
            {
                Init(GetComponent<EncounterInfo>());
            }
            DrawBounds();
            DrawCells();
        }

        private void DrawBounds()
        {
            Gizmos.color = new Color(1f, 1f, 1f, 0.25f);
            Gizmos.DrawCube(_info.gridPosition, _info.gridSize);
        }

        private void DrawCells()
        {
            Gizmos.color = Color.yellow;
            List<Vector2> worldPositions = new List<Vector2>();
            for (int y = 0; y < _info.gridDimensions.y; y++)
            {
                for (int x = 0; x < _info.gridDimensions.x; x++)
                {
                    var position = _info.GridToWorldPosition(new Vector2Int(x, y));
                    worldPositions.Add(position);
                    Gizmos.DrawSphere(position, 0.1f);
                }
            }
            DrawCellLabels(worldPositions);
        }

        private void DrawCellLabels(List<Vector2> worldPositions)
        {
            Gizmos.color = Color.cyan;
            foreach (var worldPosition in worldPositions)
            {
                var position = _info.WorldToGridPosition(worldPosition);
                Handles.Label(worldPosition, $"{position}");
            }
        }
    }
}