using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class Timer
{
    public float timerLimit, timestamp;
    public bool failOnComplete, resetOnComplete, stop;

    public Timer(float timerLimit, bool failOnComplete = true, bool resetOnComplete = true)
    {
        this.timerLimit = timerLimit;
        this.failOnComplete = failOnComplete;
        this.resetOnComplete = resetOnComplete;
    }

    public virtual void SetTimer(float time)
    {
        timestamp = time;
    }

    public virtual void Reset()
    {
        SetTimer(Time.time);
    }

    public virtual bool Check()
    {
        if (stop)
            return false;
        bool result = (Time.time - timestamp) > timerLimit;
        if (result && resetOnComplete)
            Reset();
        return result;
    }
}

public class RandomTimer : Timer
{
    float minTimerLimit, maxTimerLimit;
    public RandomTimer(float minTimerLimit, float maxTimerLimit = float.NaN, bool failOnComplete = true, bool resetOnComplete = true) :
        base(Random.Range(minTimerLimit, maxTimerLimit), failOnComplete, resetOnComplete)
    {
        this.minTimerLimit = minTimerLimit;
        this.maxTimerLimit = (float.IsNaN(minTimerLimit)) ? maxTimerLimit : minTimerLimit;
    }

    public override void SetTimer(float time)
    {
        base.SetTimer(time);
        timerLimit = Random.Range(minTimerLimit, maxTimerLimit);
    }
}

public class EventTimer : Timer
{
    GameEvent gameEvent;
    Task task;

    public EventTimer(float timerLimit, GameEvent gameEvent, bool failOnComplete=true) : base(timerLimit, failOnComplete)
    {
        this.gameEvent = gameEvent;
    }

    public void Trigger()
    {
        if (task != null && !task.IsCompleted)
            task.Dispose();
        task = Task.Delay((int)(timerLimit * 1000)).ContinueWith(t => gameEvent?.Invoke());
    }

    public void Cancel()
    {
        if (task != null)
            task.Dispose();
    }
}
