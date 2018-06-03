using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DayNightUi : MonoBehaviour
{
	private Image dayNightImg;
	private RectTransform dayNightRT;
	private DayNightController controller;
	private Text text;

	// Use this for initialization
	void Start ()
	{
		controller = GameObject.FindObjectOfType<DayNightController>();
		text = GetComponentInChildren<Text>();
		dayNightImg = transform.Find("Mask").Find("DayNight").GetComponent<Image>();
		dayNightRT = dayNightImg.GetComponent<RectTransform>();
		DayNightController.OnDayChanged += OnDayChanged;
	}
	
	// Update is called once per frame
	void Update ()
	{
		dayNightRT.rotation=Quaternion.Euler(0,0,controller.Days * 360 + 180);
	}

	void OnDayChanged(int day, bool isNight)
	{
		if (isNight)
		{
			text.text = string.Format("Night {0}", day);
			AudioPlayer.PlaySound("Nightfall");
		}
		else
		{
			text.text = string.Format("Day {0}", day);
			AudioPlayer.PlaySound("Sunrise");
		}
	}
}
