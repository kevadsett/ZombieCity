using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightingControl : MonoBehaviour
{
	[SerializeField] private GameSettings settings;
	private Light directionalLight;

	// Use this for initialization
	void Awake()
	{
		directionalLight = GetComponent<Light>();
		DayNightController.OnDayChanged += OnDayChanged;
		OnDayChanged(1,false);
	}

	void OnDayChanged(int day, bool isNight)
	{
        Debug.Log(string.Format("OnDayChanged {0} {1}", day, isNight));

        var col = isNight ? settings.nightColour : settings.dayColour;
		foreach (var cam in Camera.allCameras)
		{
			cam.backgroundColor = col;
		}

		RenderSettings.ambientLight = isNight ? settings.nightAmbient : settings.dayAmbient;
		RenderSettings.fogColor = col;
		RenderSettings.fogDensity = isNight ? settings.nightFog : settings.dayFog;

		directionalLight.enabled = !isNight;
	}
}
