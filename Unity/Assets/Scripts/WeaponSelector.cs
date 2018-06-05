using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(WeaponStorage))]
public class WeaponSelector : MonoBehaviour
{
	public delegate void WeaponChanged(WeaponSettings newWeapon);
	public static event WeaponChanged OnWeaponChanged;

	private int selectedWeaponIndex;

	private WeaponStorage weaponStorage;

	private GameObject currentWeapon;

	void Awake ()
	{
		weaponStorage = GetComponent<WeaponStorage>();
		ResetWeapon();
	}

	void Update ()
	{
		if (Input.GetKeyDown(KeyCode.LeftShift))
		{
			var prevWeaponIndex = selectedWeaponIndex;
			selectedWeaponIndex = (selectedWeaponIndex + 1) % weaponStorage.CollectedWeapons.Count;
			if (prevWeaponIndex != selectedWeaponIndex)
			{
				ChangeWeapon();
			}
		}
		else if (Input.GetKeyDown(KeyCode.RightShift))
		{
			var prevWeaponIndex = selectedWeaponIndex;
			selectedWeaponIndex = (weaponStorage.CollectedWeapons.Count + selectedWeaponIndex - 1) % weaponStorage.CollectedWeapons.Count;
			if (prevWeaponIndex != selectedWeaponIndex)
			{
				ChangeWeapon();
			}
		}
	}

    public void ResetWeapon()
    {
        selectedWeaponIndex = 0;
        ChangeWeapon();
    }

	private void ChangeWeapon()
	{
		if (currentWeapon != null)
		{
			Destroy(currentWeapon);
		}
        Debug.Log("ChangeWeapon "+selectedWeaponIndex+" "+weaponStorage);

        var nextWeapon = weaponStorage.CollectedWeapons[selectedWeaponIndex];
		Debug.Log(nextWeapon);
		currentWeapon = Instantiate(
			nextWeapon.WeaponPrefab,
			transform
		);

		var bulletSpawner = currentWeapon.GetComponentInChildren<BulletSpawner>();
		bulletSpawner.Settings = nextWeapon;

		if (OnWeaponChanged != null)
		{
			OnWeaponChanged(nextWeapon);
		}
	}
}
