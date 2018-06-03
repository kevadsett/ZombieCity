using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class AmmoTextUpdater : MonoBehaviour
{
	private WeaponSettings weaponSettings;
	private Text text;

	// Use this for initialization
	void Start()
	{
		text = GetComponent<Text>();
		WeaponSelector.OnWeaponChanged += WeaponSelectorOnOnWeaponChanged;
		BulletRaycaster.OnShotsFired += BulletRaycasterOnOnShotsFired;
	}

	private void BulletRaycasterOnOnShotsFired(List<Vector3> positions)
	{
		UpdateText();
	}

	private void OnDestroy()
	{
		WeaponSelector.OnWeaponChanged -= WeaponSelectorOnOnWeaponChanged;
		BulletRaycaster.OnShotsFired -= BulletRaycasterOnOnShotsFired;
	}

	private void WeaponSelectorOnOnWeaponChanged(WeaponSettings newWeapon)
	{
		weaponSettings = newWeapon;
		UpdateText();
	}

	public void UpdateText()
	{
		text.text = WeaponStorage.Instance.Ammo[weaponSettings.Name].ToString();
	}
}
