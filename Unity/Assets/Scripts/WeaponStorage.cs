using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponStorage : MonoBehaviour
{
	public static WeaponStorage Instance;
	public List<Weapon> CollectedWeapons = new List<Weapon>();
	public List<int> Ammo = new List<int>();

	void Start()
	{
		Instance = this;
	}
}
