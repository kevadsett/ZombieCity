using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightingControl : MonoBehaviour
{
	[SerializeField] private GameSettings settings;
	private Light directionalLight;

	// Use this for initialization
	void Start ()
	{
		directionalLight = GetComponent<Light>();
		DayNightController.OnDayChanged += OnDayChanged;
		OnDayChanged(1,false);
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}

	void OnDayChanged(int day, bool isNight)
	{
		var col = isNight ? settings.nightColour : settings.dayColour;
		Camera.main.backgroundColor = col;

		RenderSettings.ambientLight = isNight ? settings.nightAmbient : settings.dayAmbient;
		RenderSettings.fogColor = col;
		RenderSettings.fogDensity = isNight ? settings.nightFog : settings.dayFog;

		directionalLight.enabled = !isNight;
	}
}
