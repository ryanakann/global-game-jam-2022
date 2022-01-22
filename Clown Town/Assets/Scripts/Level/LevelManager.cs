using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LevelManager : PersistentSingleton<LevelManager>
{
    public Level currentLevel;

    #region EVENTS
    public UnityEvent OnLoadLevel;
    public UnityEvent<LevelStatus> OnEndLevel;
    #endregion

    public void Start()
    {
        if (Constants.debug)
        {
            LoadLevel();
        }
    }

    public void LoadLevel()
    {
        currentLevel.GenerateLanes();
        OnLoadLevel?.Invoke();
    }

    public void EndLevel(LevelStatus status)
    {
        OnEndLevel?.Invoke(status);
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
        if (Constants.debug)
        {
            Gizmos.DrawCube(currentLevel.levelArea.center, currentLevel.levelArea.size);
        }
    }
}
