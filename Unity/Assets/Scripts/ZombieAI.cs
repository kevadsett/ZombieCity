#define DEBUG_TEST


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class ZombieAI : MonoBehaviour
{
	[SerializeField] private ZombieSettings settings;
	private bool isChasing;

	private Transform player;
	private CharacterController control;
	
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
			Vector3 targetDir = player.position - transform.position;
			targetDir.y = 0;
			float step = Mathf.Deg2Rad * settings.turnSpeed * Time.deltaTime;
			Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, step, 0.0f);
			transform.rotation = Quaternion.LookRotation(newDir);
			control.SimpleMove(transform.forward * settings.chaseSpeed);
		}
	
	}

	#if DEBUG_TEST
	private void OnGUI()
	{
		float distanceToPlayer = Vector3.Distance(player.position, transform.position);
		GUILayout.Label(string.Format("Distance to player {0:0.00}",distanceToPlayer));
	}
	#endif
}

