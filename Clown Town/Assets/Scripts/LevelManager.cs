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

    public void LoadLevel()
    {
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
}
