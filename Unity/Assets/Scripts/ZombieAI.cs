#define DEBUG_TEST


using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class ZombieAI : MonoBehaviour
{
	[SerializeField] private ZombieSettings settings;
	
	
	private bool isChasing, isAttacking;
	private float attackTimer;
	private Transform player;
	private CharacterController control;

	private Vector3 lastKnownPos;
	private string debugText = "";
	
	// Use this for initialization
	void Start ()
	{
		control = GetComponent<CharacterController>();
		player = GameObject.FindGameObjectWithTag("Player").transform;		

	}
	
	// Update is called once per frame
	void Update () 
	{
		float distanceToPlayer = Vector3.Distance(player.position, transform.position);
		if (distanceToPlayer < settings.sightDistance)
		{
			isChasing = true;
		} 
		else if (distanceToPlayer > settings.lostDistance)
		{
			isChasing = false;
		}

		if (distanceToPlayer <= settings.attackRange)
		{
			isAttacking = true;
		}
		else if (distanceToPlayer > settings.attackRange * 2)
		{
			isAttacking = false;
			attackTimer = settings.attackTime;
		}

		if (isChasing)
		{
			ChasePlayer();
		}
		if (isAttacking)
		{
			PrepareAttack();
		}
	}

	private void ChasePlayer()
	{
		Vector3 targetDir = player.position - transform.position;
		RaycastHit hit;
		if (Physics.Raycast(transform.position, targetDir, out hit) && hit.transform.gameObject.tag == "Player")
		{
			debugText = "can see you";
			lastKnownPos = player.position;
		}
		else
		{
			if (Vector3.Distance(lastKnownPos, transform.position) < 0.5f)
			{
				debugText = "give up";
				isChasing = false;
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

	private void PrepareAttack()
	{
		// force not over player
		Vector3 relPos = transform.position - player.position;
		float distanceToPlayer = relPos.magnitude;
		if (distanceToPlayer < settings.attackRange)
		{
			relPos = relPos.normalized * settings.attackRange;
			transform.position = player.position + relPos;
		}

		attackTimer -= Time.deltaTime;
		debugText = string.Format("Attack {0:0.00}", attackTimer);
		if (attackTimer <= 0)
		{
			attackTimer = settings.attackTime;
			AttackPlayer();
		}
	}
	private void AttackPlayer()
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
		GUILayout.Label(string.Format("Distance to player {0:0.00} {1}",distanceToPlayer,debugText));
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.magenta;
		Gizmos.DrawWireSphere(lastKnownPos, 2);
	}
#endif
}

