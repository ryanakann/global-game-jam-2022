using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Encounters
{
    public class EncounterUnits : MonoBehaviour, IInitializable<EncounterInfo>
    {
        private EncounterInfo _info;

        public List<UnitInfo> allies;

        public List<UnitInfo> enemies;
        public List<UnitInfo>[] enemiesByRow;

        public Dictionary<(int, int), UnitInfo> alliesByGridPosition;
        public Dictionary<UnitInfo, (int, int)> gridPositionsByAlly;

        private int enemyCount;
        private int enemiesKilled;

        public UnityEvent OnAllEnemiesKilled;

        public void Init(EncounterInfo info)
        {
            _info = info;

            allies = new List<UnitInfo>();
            enemies = new List<UnitInfo>();
            alliesByGridPosition = new Dictionary<(int, int), UnitInfo>();
            gridPositionsByAlly = new Dictionary<UnitInfo, (int, int)>();

            enemiesByRow = new List<UnitInfo>[_info.gridDimensions.y];
            for (int i = 0; i < enemiesByRow.Length; i++)
            {
                enemiesByRow[i] = new List<UnitInfo>();
            }

            enemyCount = 0;
            enemiesKilled = 0;

            foreach (var wave in _info.waves)
            {
                enemyCount += wave.enemies.Count;
            }
        }

        public void AddEnemyUnit(UnitInfo unitInfo)
        {
            int row = Random.Range(0, _info.gridDimensions.y);

            var unit = Instantiate(unitInfo.gameObject);
            unitInfo = unit.GetComponent<UnitInfo>();

            unitInfo.Init();

            unitInfo.UnitType = UnitType.Foe;
            unitInfo.SetGridPosition(_info, new Vector2(_info.gridDimensions.x + 1f, row));

            enemies.Add(unitInfo);
            enemiesByRow[row].Add(unitInfo);

            // Unit should remove itself from all lists when it dies
            unitInfo.GetComponent<UnitBehavior>().OnDie.AddListener(() =>
            {
                RemoveEnemyUnit(unitInfo, false);
            });
        }

        public void RemoveEnemyUnit(UnitInfo unitInfo, bool deleteUnit)
        {
            if (enemies.Contains(unitInfo))
            {
                enemies.Remove(unitInfo);
            }

            for (int i = 0; i < enemiesByRow.Length; i++)
            {
                if (enemiesByRow[i].Contains(unitInfo))
                {
                    enemiesByRow[i].Remove(unitInfo);
                    break;
                }
            }

            enemiesKilled++;
            if (deleteUnit) Destroy(unitInfo.gameObject);
        }

        public bool AddAllyUnit(UnitInfo unitInfo, bool isInstance, Vector2 gridPosition)
        {
            var roundedGridPosition = (Mathf.RoundToInt(gridPosition.x), Mathf.RoundToInt(gridPosition.y));
            if (alliesByGridPosition.ContainsKey(roundedGridPosition))
            {
                Debug.LogError($"Cannot add unit to {gridPosition}. Space already occupied!");
                return false;
            }

            var unit = isInstance ? unitInfo.gameObject : Instantiate(unitInfo.gameObject);
            unitInfo = unit.GetComponent<UnitInfo>();

            unitInfo.Init();

            unitInfo.UnitType = UnitType.Friend;
            unitInfo.SetGridPosition(_info, gridPosition);

            allies.Add(unitInfo);
            alliesByGridPosition.Add(roundedGridPosition, unitInfo);
            gridPositionsByAlly.Add(unitInfo, roundedGridPosition);

            // Unit should remove itself from all lists when it dies
            unitInfo.GetComponent<UnitBehavior>().OnDie.AddListener(() =>
            {
                RemoveAllyUnit(unitInfo, false);
            });
            return true;
        }

        public void RemoveAllyUnit(UnitInfo unitInfo, bool deleteUnit)
        {
            if (allies.Contains(unitInfo))
            {
                allies.Remove(unitInfo);
            }

            if (gridPositionsByAlly.ContainsKey(unitInfo))
            {
                var gridPosition = gridPositionsByAlly[unitInfo];
                if (alliesByGridPosition.ContainsKey(gridPosition))
                {
                    alliesByGridPosition.Remove(gridPosition);
                }
                gridPositionsByAlly.Remove(unitInfo);
            }

            if (deleteUnit) Destroy(unitInfo.gameObject);
        }
    }
}