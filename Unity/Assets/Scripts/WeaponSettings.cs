﻿using UnityEngine;

[CreateAssetMenu(fileName = "WeaponSettings", menuName = "Settings/Weapon", order = 1)]
public class WeaponSettings : ScriptableObject
{
	public GameObject WeaponPrefab;
	public int DefaultAmmo;
	public Sprite CrossHairsSprite;
	public string Name;
	public float Range;
	public float AngleSpread;
	public int BulletsToSpawn;

	public override string ToString()
	{
		return Name;
	}
}
