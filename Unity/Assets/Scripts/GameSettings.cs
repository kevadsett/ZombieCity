using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Data", menuName = "Settings/Game", order = 2)]
public class GameSettings : ScriptableObject 
{
    public float vignetteTime=2.0f;
    public float dayDuration = 60.0f;
}
