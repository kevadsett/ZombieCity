﻿//#define DEBUG_TEST

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class ZombieAI : MonoBehaviour
{
	[SerializeField] private ZombieSettings settings;
	[SerializeField] GameObject alertIndicator;

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
	private bool isNight;

	private float gunshotAttractionTimer;

	private static int Id;
	
	// Use this for initialization
	void Start ()
	{
		control = GetComponent<CharacterController>();
		animator = GetComponentInChildren<Animator>();
		myState = State.Roaming;

		BulletRaycaster.OnEnemyHit += BulletRaycasterOnOnEnemyHit;
		DayNightController.OnDayChanged += OnDayChanged;

		name = "Zombo_" + Id;
		Id++;
		animator.SetBool("isChasing",false);
		animator.SetBool("isAttacking",false);

		BulletRaycaster.OnShotsFired += OnHeardShots;
	}

	void OnHeardShots(List<Vector3> positions, Vector3 startPosition)
	{
		if (Vector3.Distance (startPosition, transform.position) < settings.hearingDistance) {
			gunshotAttractionTimer = settings.hearingAlertnessDuration;
		}
	}

	private void OnDestroy()
	{
		BulletRaycaster.OnEnemyHit -= BulletRaycasterOnOnEnemyHit;
		BulletRaycaster.OnShotsFired -= OnHeardShots;
	}

	private void BulletRaycasterOnOnEnemyHit(string enemyid, Vector3 position)
	{
		if (enemyid != name) return;
		if (myState == State.Dying) return;

		deathStartTime = Time.time;
		myState = State.Dying;
		animator.SetTrigger("die");
		AudioPlayer.PlaySound("ZombieDie",transform.position);
	}

	// Update is called once per frame
	void Update ()
	{
		if (StateMachine.CurrentState != StateMachine.GameState.Running) return;

		if (player == null)
		{
			var playerObject = GameObject.FindGameObjectWithTag("Player");
			if (playerObject == null) return;
			player = playerObject.transform;
		}

		Vector3 vecToPlayer = player.position - transform.position;

		float distanceToPlayer = vecToPlayer.magnitude;
		bool facingPlayer = Vector3.Dot (vecToPlayer.normalized, transform.forward) > 0f;
		bool canSeePlayer = facingPlayer || distanceToPlayer < settings.forceSightDistance;

		gunshotAttractionTimer -= Time.deltaTime;

		switch (myState)
		{
			case State.Roaming:
				UpdateRoaming(distanceToPlayer, canSeePlayer);
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

		alertIndicator.SetActive (myState == State.Chasing || myState == State.Attacking);
	}

	private void UpdateRoaming(float distanceToPlayer, bool facingPlayer)
	{
		control.SimpleMove(Vector3.zero);	// move nothing: it will drop them

		var sightDist = isNight ? settings.nightSightDistance : settings.sightDistance;

		// if we can see the player OR we heard a shot and haven't forgotten about it yet...
		if ((distanceToPlayer < sightDist && facingPlayer) || gunshotAttractionTimer > 0f)
		{
			myState = State.Chasing;
			AudioPlayer.PlaySound("ZombieChase",transform.position);
		}
	}

	private void UpdateChasing(float distanceToPlayer)
	{
		ChasePlayer();

		float lostDistance = isNight ? settings.nightLostDistance : settings.lostDistance;

		// if we've lost the player AND we haven't forgotten about their gunshots
		if (distanceToPlayer > lostDistance && gunshotAttractionTimer <= 0f)
		{
			myState = State.Roaming;
		}

		if (distanceToPlayer <= settings.attackRange)
		{
			myState = State.Attacking;
			AudioPlayer.PlaySound("ZombieAttack",transform.position);
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
		float speed = isNight ? settings.nightSpeed : settings.chaseSpeed;
		float step = Mathf.Deg2Rad * settings.turnSpeed * Time.deltaTime;
		Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, step, 0.0f);
		transform.rotation = Quaternion.LookRotation(newDir);
		if (control.SimpleMove(transform.forward * speed) == false)
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
			AudioPlayer.PlaySound("ZombieAttack",transform.position);
		}
	}
	private void DamagePlayer()
	{
		Vector3 direction = (player.position - transform.position).normalized;
		// zombie lunges forward & knocks player backwards
		player.GetComponent<CharacterController>().SimpleMove(direction * settings.playerKnockback);
		control.SimpleMove(direction * settings.zombieKnockback);
		
		player.GetComponent<PlayerHealth>().Damage(transform.position);	
	}

	void OnDayChanged(int day, bool isNight)
	{
		this.isNight = isNight;
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

