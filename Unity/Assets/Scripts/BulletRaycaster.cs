using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using System.Runtime.Serialization.Formatters;
using UnityEngine;

public class BulletRaycaster : MonoBehaviour
{
	public delegate void EnemyHit(string enemyId, Vector3 position);
	public static event EnemyHit OnEnemyHit;

	public delegate void ShotsFired(List<Vector3> positions);
	public static event ShotsFired OnShotsFired;

	private Transform cameraTransform;

	private WeaponSettings weaponSettings;

	private int enemyLayerMask;

	// Use this for initialization
	private void Awake()
	{
		cameraTransform = Camera.main.transform;

		WeaponSelector.OnWeaponChanged += WeaponSelectorOnOnWeaponChanged;

		enemyLayerMask = LayerMask.GetMask("Enemy", "Building");
	}

	private void OnDestroy()
	{
		WeaponSelector.OnWeaponChanged -= WeaponSelectorOnOnWeaponChanged;
	}

	private void WeaponSelectorOnOnWeaponChanged(WeaponSettings newWeapon)
	{
		weaponSettings = newWeapon;
	}

	// Update is called once per frame
	void Update ()
	{
		if (!Input.GetMouseButtonDown(0)) return;

		if (WeaponStorage.Instance.Ammo[weaponSettings.Name] <= 0) return;

		WeaponStorage.Instance.Ammo[weaponSettings.Name]--;
		AudioPlayer.PlaySound(weaponSettings.Name);	// gun name is audio name :-)

		var endPositions = new List<Vector3>();
		for (var i = 0; i < weaponSettings.BulletsToSpawn; i++)
		{
			var rayRotation = Quaternion.Euler(new Vector3(
				Random.Range(-weaponSettings.AngleSpread, weaponSettings.AngleSpread),
				Random.Range(-weaponSettings.AngleSpread, weaponSettings.AngleSpread),
				0
			));

			var direction =  rayRotation * cameraTransform.forward;
			RaycastHit hit;
			if (Physics.Raycast(
				cameraTransform.position, direction,
				out hit,
				weaponSettings.Range,
				enemyLayerMask
			))
			{
				Debug.DrawRay(cameraTransform.position, direction * hit.distance, Color.red);

				var enemy = hit.collider.gameObject;
				if (OnEnemyHit != null)
				{
					OnEnemyHit(enemy.name, hit.point);
				}
				endPositions.Add(hit.point);
			}
			else
			{
				Debug.DrawRay(cameraTransform.position, direction * weaponSettings.Range, Color.yellow);
				endPositions.Add(cameraTransform.position + direction * weaponSettings.Range);
			}
		}

		if (OnShotsFired != null)
		{
			OnShotsFired(endPositions);
		}
	}
}
