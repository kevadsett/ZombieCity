using UnityEngine;

[CreateAssetMenu(fileName = "WeaponSettings", menuName = "Settings/Weapon", order = 1)]
public class WeaponSettings : ScriptableObject
{
	public GameObject WeaponPrefab;
	public int DefaultAmmo;
}
