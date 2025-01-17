using System.Collections.Generic;
using UnityEngine;

namespace Encounters
{
    public class EncounterInfo : Info<EncounterInfo>
    {
        [Header("Grid")]
        public Vector2Int gridDimensions;
        public Vector2 gridPosition;
        public Vector2 gridSize;

        private GameObject gridObject;

        private Bounds _bounds;
        public Bounds Bounds 
        { 
            get  
            {
                if (_bounds == null) return new Bounds(gridPosition, gridSize);
                else
                {
                    _bounds.center = gridPosition;
                    _bounds.size = gridSize;
                    return _bounds;
                }
            }
        }

        private Vector2 _cellSize;
        public Vector2 CellSize
        {
            get
            {
                if (_cellSize == null) return new Vector2(gridSize.x / gridDimensions.x, gridSize.y / gridDimensions.y);
                else
                {
                    _cellSize.x = gridSize.x / gridDimensions.x;
                    _cellSize.y = gridSize.y / gridDimensions.y;
                    return _cellSize;
                }
            }
        }

        [Header("Waves")]
        public List<EncounterWave> waves;

        public Vector2 GridToWorldPosition(Vector2 gridPosition)
        {
            return (Vector2)Bounds.min + CellSize / 2f + Vector2.Scale(CellSize, gridPosition);
        }

        public Vector2 WorldToGridPosition(Vector2 worldPosition)
        {
            return Vector2.Scale(worldPosition - (Vector2)Bounds.min - CellSize / 2f, new Vector2(1 / CellSize.x, 1 / CellSize.y));
        }

        public Vector2Int WorldToRoundedGridPosition(Vector2 worldPosition)
        {
            var gridPosition = WorldToGridPosition(worldPosition);
            return new Vector2Int(Mathf.RoundToInt(gridPosition.x), Mathf.RoundToInt(gridPosition.y));
        }

        public bool IsPointWithinGrid(Vector2 point) => Bounds.Contains(point);

        private void UpdateGridObjectTransform()
        {
            if (!gridObject)
            {
                foreach (Transform child in transform)
                {
                    if (child.name.Contains("CheckerboardGrid"))
                    {
                        gridObject = child.gameObject;
                        return;
                    }
                }

                try
                {
                    gridObject = Instantiate(Resources.Load<GameObject>("Prefabs/CheckerboardGrid"), transform);
                    gridObject.name = gridObject.name.Replace("(Clone)", "");
                }
                catch (System.NullReferenceException e)
                {
                    Debug.LogError("Prefabs/CheckerboardGrid not found. Has it been moved?\t" + e);
                }
            }

            if (gridObject.scene.rootCount == 0) return; // This is a prefab!

            gridObject.transform.SetPositionAndRotation(gridPosition, Quaternion.identity);
            gridObject.transform.SetGlobalScale(gridSize);
            gridObject.GetComponent<Renderer>()?.sharedMaterial?.SetFloat("_CellCountX", gridDimensions.x);
            gridObject.GetComponent<Renderer>()?.sharedMaterial?.SetFloat("_CellCountY", gridDimensions.y);
        }

        private void OnValidate()
        {
            UpdateGridObjectTransform();
        }

        private void Start()
        {
            if (gameObject.GetComponent<EncounterWavePopulator>())
            {
                gameObject.GetComponent<EncounterWavePopulator>().GetWave();
            }
        }
    }
}