#define DEBUG_TEST

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DayNightController : MonoBehaviour
{
    [SerializeField] private GameSettings settings;
    public float Days { get; private set; }

    private void Start()
    {
        Days = 1;
    }

    private void Update()
    {
        Days += Time.deltaTime / settings.dayDuration;
#if DEBUG_TEST
        if (Input.GetKeyDown(KeyCode.K))    Days-=0.1f;
        if (Input.GetKeyDown(KeyCode.L))    Days+=0.1f;    
#endif
    }
}

