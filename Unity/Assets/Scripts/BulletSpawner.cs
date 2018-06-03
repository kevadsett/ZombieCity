using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSpawner : MonoBehaviour
{
	public GameObject BulletPrefab;

	public int WeaponIndex;
	public WeaponSettings Settings;

	private Transform playerTransform;
	private Transform cameraTransform;

	void Start()
	{
		playerTransform = GameObject.Find("FPSController").transform;
		cameraTransform = GameObject.Find("FirstPersonCharacter").transform;
	}

	void Update()
	{
		if (!Input.GetMouseButtonDown(0)) return;

		if (WeaponStorage.Instance.Ammo[WeaponIndex] == 0)
		{
			return;
		}

		WeaponStorage.Instance.Ammo[WeaponIndex]--;

		for (var i = 0; i < Settings.BulletsToSpawn; i++)
		{
			var rotation = Quaternion.Euler(
				cameraTransform.rotation.eulerAngles.x + Random.Range(-Settings.AngleSpread, Settings.AngleSpread),
				playerTransform.rotation.eulerAngles.y + Random.Range(-Settings.AngleSpread, Settings.AngleSpread),
				0
			);

			var bulletObject = Instantiate(
				BulletPrefab,
				transform.position,
				rotation
			);

			bulletObject.GetComponent<BulletMovement>().MaxDistBeforeDestroy = Settings.Range;
		}
	}
}
