using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorchController : MonoBehaviour
{
	private Light torch;

	// Use this for initialization
	void Start ()
	{
		torch = GetComponent<Light>();
		DayNightController.OnDayChanged += OnDayChanged;
		OnDayChanged(1,false);

	}
	
	void OnDayChanged(int day, bool isNight)
	{
		torch.enabled = isNight;
	}
}
