using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(WeaponStorage))]
public class WeaponSelector : MonoBehaviour
{
	private int selectedWeaponIndex;

	private WeaponStorage weaponStorage;

	private GameObject currentWeapon;

	void Start ()
	{
		weaponStorage = GetComponent<WeaponStorage>();
		ChangeWeapon();
	}

	void Update ()
	{
		if (Input.GetAxis("Mouse ScrollWheel") > 0f)
		{
			var prevWeaponIndex = selectedWeaponIndex;
			selectedWeaponIndex = (selectedWeaponIndex + 1) % weaponStorage.CollectedWeapons.Count;
			if (prevWeaponIndex != selectedWeaponIndex)
			{
				ChangeWeapon();
			}
		}
		else if (Input.GetAxis("Mouse ScrollWheel") < 0f)
		{
			var prevWeaponIndex = selectedWeaponIndex;
			selectedWeaponIndex = (selectedWeaponIndex - 1) % weaponStorage.CollectedWeapons.Count;
			if (prevWeaponIndex != selectedWeaponIndex)
			{
				ChangeWeapon();
			}
		}
	}

	private void ChangeWeapon()
	{
		if (currentWeapon != null)
		{
			Destroy(currentWeapon);
		}

		Debug.Log(selectedWeaponIndex + ", " + weaponStorage.CollectedWeapons.Count);
		var nextWeapon = weaponStorage.CollectedWeapons[selectedWeaponIndex];
		currentWeapon = Instantiate(
			nextWeapon.Settings.WeaponPrefab,
			transform
		);
	}
}
