//#define DEBUG_TEST

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class ZombieAI : MonoBehaviour
{
	[SerializeField] private ZombieSettings settings;

	private enum State
	{
		Roaming,
		Chasing,
		Attacking,
		Dying
	}

	private State myState;
	
	private float attackTimer;
	private Transform player;
	private CharacterController control;
	private Animator animator;

	private Vector3 lastKnownPos;
	private string debugText = "";

	private float deathStartTime;

	private static int Id;
	
	// Use this for initialization
	void Start ()
	{
		control = GetComponent<CharacterController>();
		animator = GetComponentInChildren<Animator>();
		player = GameObject.FindGameObjectWithTag("Player").transform;
		myState = State.Roaming;

		BulletRaycaster.OnEnemyHit += BulletRaycasterOnOnEnemyHit;

		name = "Zombo_" + Id;
		Id++;
		animator.SetBool("isChasing",false);
		animator.SetBool("isAttacking",false);
	}

	private void OnDestroy()
	{
		BulletRaycaster.OnEnemyHit -= BulletRaycasterOnOnEnemyHit;
	}

	private void BulletRaycasterOnOnEnemyHit(string enemyid, Vector3 position)
	{
		if (enemyid != name) return;

		deathStartTime = Time.time;
		myState = State.Dying;
		animator.SetTrigger("die");

		Debug.Log(name + " HIT!");
	}

	// Update is called once per frame
	void Update () 
	{
		var distanceToPlayer = Vector3.Distance(player.position, transform.position);
		switch (myState)
		{
			case State.Roaming:
				UpdateRoaming(distanceToPlayer);
				animator.SetBool("isChasing",false);
				animator.SetBool("isAttacking",false);
				break;
			case State.Chasing:
				UpdateChasing(distanceToPlayer);
				animator.SetBool("isChasing",true);
				animator.SetBool("isAttacking",false);
				break;
			case State.Attacking:
				UpdateAttacking(distanceToPlayer);
				animator.SetBool("isChasing",false);
				animator.SetBool("isAttacking",true);
				break;
			case State.Dying:
				UpdateDying();
				break;
			default:
				throw new ArgumentOutOfRangeException("State " + myState + " not recognised.");
		}
	}

	private void UpdateRoaming(float distanceToPlayer)
	{
		control.SimpleMove(Vector3.zero);	// move nothing: it will drop them

		if (distanceToPlayer < settings.sightDistance)
		{
			myState = State.Chasing;	
		}
	}

	private void UpdateChasing(float distanceToPlayer)
	{
		ChasePlayer();

		if (distanceToPlayer > settings.lostDistance)
		{
			myState = State.Roaming;
		}

		if (distanceToPlayer <= settings.attackRange)
		{
			myState = State.Attacking;
		}
	}

	private void UpdateAttacking(float distanceToPlayer)
	{
		AttackPlayer();

		if (distanceToPlayer > settings.attackRange * 2)
		{
			myState = State.Chasing;
			attackTimer = 0;
		}
	}

	private void UpdateDying()
	{
		if (Time.time - deathStartTime >= settings.deathDuration)
		{
			Destroy(gameObject);
			// gav-style pop
		}
		else
		{
			control.SimpleMove(Vector3.zero);	// move nothing: it will drop them
		}
	}

	private void ChasePlayer()
	{
		Vector3 targetDir = player.position - transform.position;
		RaycastHit hit;
		if (Physics.Raycast(transform.position, targetDir, out hit) && hit.transform.gameObject.CompareTag("Player"))
		{
			debugText = "can see you";
			lastKnownPos = player.position;
		}
		else
		{
			if (Vector3.Distance(lastKnownPos, transform.position) < 0.5f)
			{
				debugText = "give up";
				myState = State.Roaming;
				return;
			}
			else
			{
				debugText = "going for last pos";
				targetDir = lastKnownPos - transform.position;
			}
		}

		targetDir.y = 0;
		// move and turn
		float step = Mathf.Deg2Rad * settings.turnSpeed * Time.deltaTime;
		Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, step, 0.0f);
		transform.rotation = Quaternion.LookRotation(newDir);
		if (control.SimpleMove(transform.forward * settings.chaseSpeed) == false)
		{
			debugText = "hitwall";
		}
	}

	private void AttackPlayer()
	{
		// force not over player
		Vector3 relPos = transform.position - player.position;
		float distanceToPlayer = relPos.magnitude;
		if (distanceToPlayer < settings.attackRange)
		{
			relPos = relPos.normalized * settings.attackRange;
			transform.position = player.position + relPos;
		}

		var oldTime = attackTimer;
		attackTimer += Time.deltaTime;
		debugText = string.Format("Attack {0:0.00}", attackTimer);
		// check if the attack takes place:
		// we have a extra timer to spot the moment of the attack 
		if (attackTimer >= settings.attackDamageAt && oldTime < settings.attackDamageAt)
		{
			DamagePlayer();
		}
		if (attackTimer >= settings.attackTime)	// reset
		{
			attackTimer = 0;
		}
	}
	private void DamagePlayer()
	{
		Vector3 direction = (player.position - transform.position).normalized;
		// zombie lunges forward & knocks player backwards
		var pmove = player.GetComponent<CharacterController>().SimpleMove(direction * settings.playerKnockback);
		var zmove = control.SimpleMove(direction * settings.zombieKnockback);
		Debug.Log(string.Format("knockback {0} {1}", pmove, zmove));
		
		player.GetComponent<PlayerHealth>().Damage(transform.position);	
	}

#if DEBUG_TEST
	private void OnGUI()
	{
		float distanceToPlayer = Vector3.Distance(player.position, transform.position);
//		GUILayout.Label(string.Format("Distance to player {0:0.00} Scans {1:0.00} {2:0.00} {3:0.00} {4}",distanceToPlayer,leftDist,forwardDist,rightDist,debugText));
		GUILayout.Label(string.Format("Distance to player {0:0.00} {1} {2} ",
			distanceToPlayer,myState,debugText));
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.magenta;
		Gizmos.DrawWireSphere(lastKnownPos, 2);
	}
#endif
}

