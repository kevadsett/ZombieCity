//#define DEBUG_TEST
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
	public delegate void HealthLost(int newHealth);
	public static event HealthLost OnHealthLost;

	private const int DEFAULT_HEALTH = 5;
	private InjuryEffect injury;

	public int Health { get; private set; }
	// Use this for initialization
	private void Awake()
	{
		Health = DEFAULT_HEALTH;
		injury = GameObject.FindObjectOfType<InjuryEffect>();
	}

	public void Heal(int amount)
	{
		Health += amount;
		if (Health > DEFAULT_HEALTH)
			Health = DEFAULT_HEALTH;
	}

	public void ResetHealth()
	{
		Health = DEFAULT_HEALTH;
		injury.Clear();
	}

	public void Damage(Vector3 fromPos)
	{
		Health--;
		
		if (Health > 0)
		{
			AudioPlayer.PlaySound("Hurt");
		}
		else if (Health == 0)
		{
			AudioPlayer.PlaySound("Die");
		}

		// determine the angle of attack
		// the Vector3.Angle gives abs angle so comparing to left/right
		var dir = fromPos - transform.position;
		var angFront = Vector3.Angle(transform.forward, dir);
		var angLeft = Vector3.Angle(-transform.right, dir);
		var angRight = Vector3.Angle(transform.right, dir);
		//Debug.Log(string.Format("Hit angle {0:0.00}",ang));
		if (angFront < angLeft && angFront < angRight) // front
		{
			injury.Injury(true,true);			
		}
		else
		{
			if (angLeft < angRight)
			{
				injury.Injury(true, false);
			}
			else
			{
				injury.Injury(false, true);
			}
		}

		if (OnHealthLost != null)
		{
			OnHealthLost(Health);
		}
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
