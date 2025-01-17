using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Cell : MonoBehaviour
{
    public int index;
    public Lane lane;
    public Unit unit;

    public void Init(Lane lane, int cellIndex, float cellWidth, float cellHeight, int cellCount)
    {
        this.lane = lane;
        index = cellIndex;

        name = $"{lane.name} - Cell {cellIndex}";
        transform.localScale = new Vector3(cellWidth, cellHeight);
        float t = (cellIndex + 0.5f) / cellCount;
        transform.position = lane.Lerp(t);
        transform.SetParent(lane.transform);

        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.color = (cellIndex + lane.index % 2) % 2 == 0 ? Color.white : Color.clear;
    }

    public bool AddUnit(GameObject unit)
    {
        if (this.unit != null)
        {
            Debug.LogError($"Cannot add unit to an already-occupied cell!");
            return false;
        }


        Unit instance = unit.GetComponent<Unit>();

        instance.transform.SetParent(transform);
        instance.transform.position = transform.position;


        instance.Place(lane);
        this.unit = instance;
        return true;
    }

    public void RemoveUnit()
    {
        if (unit == null)
        {
            Debug.LogError($"Cannot remove unit from an empty cell!");
            return;
        }

        unit.DieStart();
        unit = null;
    }
}
