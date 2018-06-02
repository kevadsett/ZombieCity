using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "Settings/Zombie", order = 1)]
public class ZombieSettings : ScriptableObject 
{
	public float sightDistance=10,lostDistance=30;
	public float chaseSpeed=2,turnSpeed=30;
	public float evadeDistance = 2;
}
