using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;

public class LevelManager : PersistentSingleton<LevelManager>
{
    public LevelInfo currentLevelInfo;
    public List<Lane> lanes;
    public GameObject lanePrefab;

    #region EVENTS
    public UnityEvent OnLoadLevel;
    public UnityEvent<LevelStatus> OnEndLevel;
    #endregion

    private void Start()
    {
        if (Constants.debug)
        {
            LoadLevel();
        }
    }

    private void Update()
    {
        if (Constants.debug)
        {
            if (Input.GetMouseButtonDown(0))
            {
                PlaceUnit(Resources.Load<GameObject>("Prefabs/Units/Cow"));
            }
            else if (Input.GetMouseButtonDown(1))
            {
                RemoveUnit();
            }
        }
    }

    public void PlaceUnit(GameObject unit)
    {
        if (unit.GetComponent<Unit>() == null)
        {
            Debug.LogError($"Provided GameObject ({unit.name}) must have Unit component.");
            return;
        }

        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Lane lane = lanes.OrderBy(l => Vector3.Distance(l.transform.position, mousePosition)).First();
        int cellIndex = lane.cells.IndexOf(lane.cells.OrderBy(c => Vector3.Distance(c.transform.position, mousePosition)).First());
        lane.AddUnit(unit, cellIndex);
    }

    public void RemoveUnit()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Lane lane = lanes.OrderBy(l => Vector3.Distance(l.transform.position, mousePosition)).First();
        int cellIndex = lane.cells.IndexOf(lane.cells.OrderBy(c => Vector3.Distance(c.transform.position, mousePosition)).First());
        lane.RemoveUnit(cellIndex);
    }

    public Vector2 WorldToPlayNormalizedAreaPoint(Vector2 worldPos)
    {
        Vector2 min = currentLevelInfo.details.playArea.min;
        Vector2 max = currentLevelInfo.details.playArea.max;

        return new Vector2(Mathf.InverseLerp(min.x, max.x, worldPos.x), Mathf.InverseLerp(min.y, max.y, worldPos.y));
    }

    /// <summary>
    /// given some normalized position within the play area, will return the nearest cell.
    /// </summary>
    /// <param name="point"></param>
    /// <returns>(laneIndex, cellIndex)</returns>
    public (int, int) NormalizedAreaPointToNearestCell(Vector2 point)
    {
        int laneIndex = Mathf.RoundToInt(point.y * currentLevelInfo.laneCount);
        int cellIndex = Mathf.RoundToInt(point.x * currentLevelInfo.cellCountPerLane);
        return (laneIndex, cellIndex);
    }

    public void LoadLevel()
    {
        GenerateLanes();
        OnLoadLevel?.Invoke();
    }

    public void EndLevel(LevelStatus status)
    {
        OnEndLevel?.Invoke(status);
    }

    public void GenerateLanes()
    {
        Transform parent = new GameObject("Lanes").transform;
        lanes = new List<Lane>();
        for (int i = 0; i < currentLevelInfo.laneCount; i++)
        {
            Lane lane = Instantiate(lanePrefab).GetComponent<Lane>();
            lane.Init(i, currentLevelInfo);
            lanes.Add(lane);
            lane.transform.SetParent(parent);
        }
    }

    public enum LevelStatus
    {
        Victory,
        Defeat,
        Retreat,
        Draw
    }

    private void OnDrawGizmos()
    {
        if (currentLevelInfo.details.showDebugArea)
        {
            Gizmos.DrawCube(currentLevelInfo.details.playArea.center, currentLevelInfo.details.playArea.size);
        }
    }
}
