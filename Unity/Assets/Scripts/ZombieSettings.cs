using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "Settings/Zombie", order = 1)]
public class ZombieSettings : ScriptableObject
{
    [Header("Detection/Movement")] 
    public float sightDistance = 10;
	public float forceSightDistance = 2;
    public float lostDistance=10;
	public float nightLostDistance=20;
    public float chaseSpeed=2,turnSpeed=30;
    public float nightSightDistance = 15;
    public float nightSpeed = 3;
	public float hearingDistance = 25;
	public float hearingAlertnessDuration = 1;

    [Header("Attack")]
    [Tooltip("range at which the zombie attacks")]
    public float attackRange = 1.0f;
    [Tooltip("Amount of time for attack animation")]
    public float attackTime = 1.0f;
    [Tooltip("time within the animation that the damage is applied")]
    public float attackDamageAt = 1.0f;
    public float playerKnockback = 100, zombieKnockback = 50;
    public float deathDuration = 1f;
}
