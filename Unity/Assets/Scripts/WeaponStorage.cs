using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponStorage : MonoBehaviour
{
	public static WeaponStorage Instance;
	public List<WeaponSettings> CollectedWeapons = new List<WeaponSettings>();

	// TODO: Don't hard code this
	public Dictionary<string, int> Ammo = new Dictionary<string, int>
	{
		{"Pistol", 12},
		{"Shotgun", 6}
	};

	private void Awake()
	{
		Instance = this;
	}

    public void ResetAmmo()
    {
        Ammo.Clear();
        foreach(var wep in CollectedWeapons)
        {
            Ammo[wep.Name] = wep.DefaultAmmo;
        }
    }
}
