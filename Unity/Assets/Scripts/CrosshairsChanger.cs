using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class CrosshairsChanger : MonoBehaviour
{
	private Image image;

	void Start ()
	{
		image = GetComponent<Image>();
		WeaponSelector.OnWeaponChanged += WeaponSelectorOnOnWeaponChanged;
	}

	private void OnDestroy()
	{
		WeaponSelector.OnWeaponChanged -= WeaponSelectorOnOnWeaponChanged;
	}

	private void WeaponSelectorOnOnWeaponChanged(Weapon newWeapon)
	{
		image.sprite = newWeapon.Settings.CrossHairsSprite;
	}
}
