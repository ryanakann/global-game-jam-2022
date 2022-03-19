using System.Collections;
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

        public void Init(EncounterInfo info)
        {
            _info = info;
        }

        private void OnDrawGizmos()
        {
            if (!enabled) return;
            if (!_info)
            {
                Init(GetComponent<EncounterInfo>());
            }
            DrawBounds();
            DrawCellCenters();
        }

        private void DrawBounds()
        {
            Gizmos.color = new Color(1f, 1f, 1f, 0.25f);
            Gizmos.DrawCube(_info.gridPosition, _info.gridSize);
        }

        private void DrawCellCenters()
        {
            Gizmos.color = Color.yellow;
            List<Vector2> worldPositions = new List<Vector2>();
            for (int y = 0; y < _info.gridCells.y; y++)
            {
                for (int x = 0; x < _info.gridCells.x; x++)
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