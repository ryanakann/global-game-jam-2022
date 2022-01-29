using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Lane : MonoBehaviour {
    public int index;

    public Vector2 start;
    public Vector2 end;

    public float laneLength { get; private set; }
    public float laneWidth { get; private set; }
    public float cellWidth { get; private set; }

    protected List<GameObject> objects { get; }//indexed by grid position within lane

    public List<Unit> allies;
    public List<Unit> enemies;

    public GameObject cellPrefab;

    public List<Cell> cells;

    public GameObject obj;

    public void Init(int laneIndex, LevelInfo levelInfo)
    {
        index = laneIndex;

        laneLength = levelInfo.details.playArea.size.x;
        laneWidth = levelInfo.details.playArea.size.y / levelInfo.laneCount;
        cellWidth = laneLength / levelInfo.cellCountPerLane;

        Bounds playArea = levelInfo.details.playArea;

        start = new Vector2(playArea.min.x, playArea.min.y + (0.5f + laneIndex) * laneWidth);
        end = start + Vector2.right * laneLength;

        gameObject.name = $"Lane {laneIndex}";
        transform.position = (start + end) / 2f;

        allies = new List<Unit>();
        enemies = new List<Unit>();

        cells = new List<Cell>();
        for (int cellIndex = 0; cellIndex < levelInfo.cellCountPerLane; cellIndex++)
        {
            Cell cell = Instantiate(cellPrefab).GetComponent<Cell>();
            cell?.Init(this, cellIndex, cellWidth, laneWidth, levelInfo.cellCountPerLane);

            cells.Add(cell.GetComponent<Cell>());
        }
    }

    public bool AddUnit(GameObject unit, int cellIndex, bool isInstance)
    {
        if (cellIndex >= cells.Count)
        {
            Debug.LogError($"Provided cellIndex ({cellIndex}) must be within range ({0}, {cells.Count})");
            return false;
        }

        // Enemy spawn off the screen
        if (cellIndex < 0)
        {
            Unit instance = isInstance ? unit.GetComponent<Unit>() : Instantiate(unit).GetComponent<Unit>();

            instance.transform.SetParent(transform);
            instance.transform.position = end + Vector2.right * cellWidth;

            instance.Place(this);
            enemies.Add(instance);
            instance.OnDie.AddListener(() => enemies.Remove(instance));
        }
        else
        {
            Cell cell = cells[cellIndex];

            GameObject ally = isInstance ? unit : Instantiate(unit);
            if (!cell.AddUnit(ally))
            {
                Destroy(ally);
                return false;
            }
            Unit instance = ally.GetComponent<Unit>();
            allies.Add(instance);
            instance.OnDie.AddListener(() => allies.Remove(instance));
        }
        return true;
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
