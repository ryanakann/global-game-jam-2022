using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Lane : MonoBehaviour {
    public int index;

    public Vector2 start;
    public Vector2 end;
    protected List<GameObject> objects { get; }//indexed by grid position within lane

    public GameObject cellPrefab;

    public List<Cell> cells;

    public GameObject obj;

    public void Init(int laneIndex, LevelInfo levelInfo)
    {
        index = laneIndex;

        float laneLength = levelInfo.details.playArea.size.x;
        float laneWidth = levelInfo.details.playArea.size.y / levelInfo.laneCount;
        float cellWidth = laneLength / levelInfo.cellCountPerLane;

        Bounds playArea = levelInfo.details.playArea;

        start = new Vector2(playArea.min.x, playArea.min.y + (0.5f + laneIndex) * laneWidth);
        end = start + Vector2.right * laneLength;

        gameObject.name = $"Lane {laneIndex}";
        transform.position = (start + end) / 2f;

        cells = new List<Cell>();
        for (int cellIndex = 0; cellIndex < levelInfo.cellCountPerLane; cellIndex++)
        {
            Cell cell = Instantiate(cellPrefab).GetComponent<Cell>();
            cell?.Init(this, cellIndex, cellWidth, laneWidth, levelInfo.cellCountPerLane);

            cells.Add(cell.GetComponent<Cell>());
        }
    }

    public void AddUnit(GameObject unit, int cellIndex)
    {
        Cell cell = cells[cellIndex];
        if (cellIndex < 0 || cellIndex >= cells.Count)
        {
            Debug.LogError($"Provided cellIndex ({cellIndex}) must be within range ({0}, {cells.Count})");
            return;
        }

        cell.AddUnit(unit);
    }

    public void RemoveUnit(int cellIndex)
    {
        Cell cell = cells[cellIndex];
        if (cellIndex < 0 || cellIndex >= cells.Count)
        {
            Debug.LogError($"Provided cellIndex ({cellIndex}) must be within range ({0}, {cells.Count})");
            return;
        }

        cell.RemoveUnit();
    }

    public int GridIndex(Vector2 position) {
        return 0;
    }

    public Vector2 Lerp(float t)
    {
        return Vector2.Lerp(start, end, t);
    }

    public float InverseLerp(Vector2 position)
    {
        Vector3 AB = end - start;
        Vector3 AV = position - start;
        return Vector3.Dot(AV, AB) / Vector3.Dot(AB, AB);
    }
}
