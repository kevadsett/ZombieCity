using System.Collections;
using System.Collections.Generic;
using System.Xml.Schema;
using UnityEngine;
using UnityEngine.UI;

public class InjuryEffect : MonoBehaviour
{
	[SerializeField] private GameSettings settings;
	private Image[] images;
	private float leftTimer, rightTimer;

	// Use this for initialization
	void Start ()
	{
		images = GetComponentsInChildren<Image>();
		var col=new Color(1,1,1,0);
		foreach (var img in images)
		{
			img.color = col;
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (leftTimer > 0)
		{
			leftTimer = Mathf.Max(0, leftTimer - Time.deltaTime);
			var col = new Color(1,1,1,leftTimer/settings.vignetteTime);
			images[0].color = col;
		}
		if (rightTimer > 0)
		{
			rightTimer = Mathf.Max(0, rightTimer - Time.deltaTime);
			var col = new Color(1,1,1,rightTimer/settings.vignetteTime);
			images[1].color = col;
		}		
	}

	public void Injury(bool left, bool right)
	{
		if (left)
		{
			leftTimer = settings.vignetteTime;
		}

		if (right)
		{
			rightTimer = settings.vignetteTime;
		}
		
	}
}
