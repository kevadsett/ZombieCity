using UnityEngine;

[CreateAssetMenu(fileName = "WeaponSettings", menuName = "Settings/Weapon", order = 1)]
public class WeaponSettings : ScriptableObject
{
	public GameObject WeaponPrefab;
	public int DefaultAmmo;
	public Sprite CrossHairsSprite;
	public string Name;

	public override string ToString()
	{
		return Name;
	}
}
