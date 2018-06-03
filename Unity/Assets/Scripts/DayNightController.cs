#define DEBUG_TEST

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DayNightController : MonoBehaviour
{
    public delegate void DayNightChanged(int day, bool isNight);
    public static event DayNightChanged OnDayChanged;    
    
    [SerializeField] private GameSettings settings;
    public float Days { get; private set; }

    private void Start()
    {
        Days = 1;
    }

    private void Update()
    {
        float oldDay = Days;
        Days += Time.deltaTime / settings.dayDuration;        
#if DEBUG_TEST
        if (Input.GetKeyDown(KeyCode.K))    Days-=0.1f;
        if (Input.GetKeyDown(KeyCode.L))    Days+=0.1f;    
#endif
        if (Mathf.FloorToInt(oldDay * 2) != Mathf.FloorToInt(Days * 2))
        {
            int day = Mathf.FloorToInt(Days);
            bool isDay = (Days - day < 0.5f);
            OnDayChanged(day, !isDay);
        }
        
    }
}

