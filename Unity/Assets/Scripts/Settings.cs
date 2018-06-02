using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "Settings/Zombie", order = 1)]
public class ZombieSettings : ScriptableObject 
{
	public float sightDistance=10,lostDistance=30;
	public float chaseSpeed=2,turnSpeed=30;
	public float attackRange = 1.0f, attackTime = 1.0f;
	public float playerKnockback = 100, zombieKnockback = 50;
}

[CreateAssetMenu(fileName = "Data", menuName = "Settings/Game", order = 2)]
public class GameSettings : ScriptableObject 
{
	public float vignetteTime=2.0f;
}
