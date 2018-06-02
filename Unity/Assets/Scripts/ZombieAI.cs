#define DEBUG_TEST


using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class ZombieAI : MonoBehaviour
{
	[SerializeField] private ZombieSettings settings;
	
	
	private bool isChasing;
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

		if (distanceToPlayer > settings.lostDistance)
		{
			isChasing = false;
		}

		if (isChasing)
		{
			ChasePlayer();
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

