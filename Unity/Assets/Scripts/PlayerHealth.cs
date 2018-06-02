//#define DEBUG_TEST
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
	private const int DEFAULT_HEALTH = 5;
	private InjuryEffect injury;

	public int Health { get; private set; }
	// Use this for initialization
	void Start ()
	{
		Health = DEFAULT_HEALTH;
		injury = GameObject.FindObjectOfType<InjuryEffect>();
	}

	public void Damage(Vector3 fromPos)
	{
		Health--;
		injury.Injury(true,true);
	}
	
#if DEBUG_TEST
	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.P))
		{
			Health++;
			Debug.Log("Health is "+Health);	
		}
		if (Input.GetKeyDown(KeyCode.O))
		{
			Health--;
			Debug.Log("Health is "+Health);
		}
	}
#endif

}
