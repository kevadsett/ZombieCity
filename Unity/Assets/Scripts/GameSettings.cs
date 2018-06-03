using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Data", menuName = "Settings/Game", order = 2)]
public class GameSettings : ScriptableObject 
{
    public float vignetteTime = 2.0f;
    public float dayDuration = 60.0f;
    public Color dayColour = Color.cyan;
    public Color nightColour = Color.black;
    public Color dayAmbient = Color.grey;
    public Color nightAmbient = Color.black;
    public float dayFog = 0.01f, nightFog = 0.05f;
}
