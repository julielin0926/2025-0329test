using UnityEngine;
using UnityEngine.Events;

public class TimeWindController : MonoBehaviour
{
    public enum TimePeriod { Morning, Noon, Evening }
    public enum WindDirection { None, East, West }

    public TimePeriod currentTime = TimePeriod.Morning;
    public WindDirection currentWind = WindDirection.East;

    public float timePerPeriod = 5f; // 每段持續時間（秒）
    private float timer;

    public UnityEvent<TimePeriod, WindDirection> OnTimeWindChanged;

    void Start()
    {
        timer = timePerPeriod;
        UpdateWindByTime();
        OnTimeWindChanged?.Invoke(currentTime, currentWind);
    }

    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            timer = timePerPeriod;
            AdvanceTime();
        }
    }

    void AdvanceTime()
    {
        currentTime = (TimePeriod)(((int)currentTime + 1) % 3); // 早→中→晚→早...
        UpdateWindByTime();
        OnTimeWindChanged?.Invoke(currentTime, currentWind);
    }

    void UpdateWindByTime()
    {
        switch (currentTime)
        {
            case TimePeriod.Morning:
                currentWind = WindDirection.East;
                break;
            case TimePeriod.Noon:
                currentWind = WindDirection.None;
                break;
            case TimePeriod.Evening:
                currentWind = WindDirection.West;
                break;
        }
    }
}
