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
	}
	
	// Update is called once per frame
	void Update ()
	{
		dayNightRT.rotation=Quaternion.Euler(0,0,controller.Days * -360);
		int day = Mathf.FloorToInt(controller.Days);
		float delta = controller.Days - day;
		if (delta < 0.5f)
		{
			text.text = string.Format("Day {0}", day);
		}
		else
		{
			text.text = string.Format("Night {0}", day);
		}
	}
}
