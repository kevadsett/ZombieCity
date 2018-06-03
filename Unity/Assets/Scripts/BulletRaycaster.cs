using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletRaycaster : MonoBehaviour
{
	public delegate void EnemyHit(string enemyId, Vector3 position);
	public static event EnemyHit OnEnemyHit;

	private Transform cameraTransform;

	private float weaponRange;

	private int enemyLayerMask;

	// Use this for initialization
	private void Awake()
	{
		cameraTransform = Camera.main.transform;

		WeaponSelector.OnWeaponChanged += WeaponSelectorOnOnWeaponChanged;

		enemyLayerMask = LayerMask.GetMask("Enemy");
	}

	private void OnDestroy()
	{
		WeaponSelector.OnWeaponChanged -= WeaponSelectorOnOnWeaponChanged;
	}

	private void WeaponSelectorOnOnWeaponChanged(Weapon newWeapon)
	{
		weaponRange = newWeapon.Settings.Range;
	}

	// Update is called once per frame
	void Update () {
		RaycastHit hit;
		if (Physics.Raycast(
			cameraTransform.position, cameraTransform.forward,
			out hit,
			weaponRange,
			enemyLayerMask
		))
		{
			Debug.DrawRay(cameraTransform.position, cameraTransform.forward * hit.distance, Color.red);

			if (!Input.GetMouseButtonDown(0)) return;

			var enemy = hit.collider.gameObject;
			if (OnEnemyHit != null)
			{
				OnEnemyHit(enemy.name, hit.point);
			}
		}
		else
		{
			Debug.DrawRay(cameraTransform.position, cameraTransform.forward * weaponRange, Color.yellow);
		}
	}
}
