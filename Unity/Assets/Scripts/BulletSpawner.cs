using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSpawner : MonoBehaviour
{
	public GameObject BulletPrefab;

	public int BulletsToSpawn = 1;
	public float AngleSpread = 10f;

	private Transform playerTransform;
	private Transform cameraTransform;

	void Start()
	{
		playerTransform = GameObject.Find("FPSController").transform;
		cameraTransform = GameObject.Find("FirstPersonCharacter").transform;
	}

	void Update()
	{
		if (Input.GetMouseButtonDown(0))
		{
			for (var i = 0; i < BulletsToSpawn; i++)
			{
				var rotation = Quaternion.Euler(
					cameraTransform.rotation.eulerAngles.x + Random.Range(-AngleSpread, AngleSpread),
					playerTransform.rotation.eulerAngles.y + Random.Range(-AngleSpread, AngleSpread),
					0
				);

				Instantiate(
					BulletPrefab,
					transform.position,
					rotation
				);
			}
		}
	}
}
