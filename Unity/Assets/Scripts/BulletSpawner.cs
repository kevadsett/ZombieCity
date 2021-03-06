﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSpawner : MonoBehaviour
{
	public GameObject BulletPrefab;

	public WeaponSettings Settings;

	void Start()
	{
		BulletRaycaster.OnShotsFired += BulletRaycasterOnOnShotsFired;
	}

	private void OnDestroy()
	{
		BulletRaycaster.OnShotsFired -= BulletRaycasterOnOnShotsFired;
	}

	private void BulletRaycasterOnOnShotsFired(List<Vector3> positions, Vector3 startPosition)
	{
		foreach (var position in positions)
		{
			var bulletObject = Instantiate(BulletPrefab);
			bulletObject.transform.position = transform.position;

			var bulletMovement = bulletObject.GetComponent<BulletMovement>();

			bulletMovement.TargetPosition = position;
		}
	}
}
