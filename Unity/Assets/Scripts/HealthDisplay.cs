using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using UnityEngine;
using UnityEngine.UI;

public class HealthDisplay : MonoBehaviour
{
	private PlayerHealth health;
	private int lastHealth;
	private Image[] images;

	// Use this for initialization
	void Start ()
	{
		health = GameObject.FindObjectOfType<PlayerHealth>();
		images = GetComponentsInChildren<Image>();
		SetHealth();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (health.Health != lastHealth)
		{
			SetHealth();
		}
	}

	void SetHealth()
	{
		for (int i = 0; i < images.Length; i++)
		{
			images[i].enabled = (i < health.Health);
		}

		lastHealth = health.Health;
	}
}
