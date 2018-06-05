using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour {

	public float clearRadius;
	public float minScale;
	public float maxScale;

	public Collider doorCollider;
	public GameObject lootableIndicator;
	public GameObject unlootableIndicator;

	public bool hasBeenLooted { get; private set; }

	[System.Serializable] struct LootItem {
		public enum ItemType {
			Nothing, Food, Zombie, Pistol, Shotgun
		}

		public ItemType type;
		public int chanceMultiplier;
		public int minQuantity;
		public int maxQuantity;
		public int numGuardZombies;
	}

	[SerializeField] List<LootItem> lootItems;

	LootItem itemToLoot;
	int itemQuantity;

	public int NumZombiesToSpawn { get { 
			return itemToLoot.numGuardZombies;
		}
	}

	void Awake () {
		SelectItem ();
	}

	void SelectItem () {
		int maxChance = 0;
		for (int i = 0; i < lootItems.Count; i++) {
			maxChance += lootItems[i].chanceMultiplier;
		}

		itemToLoot = lootItems[0];

		int chance = Random.Range (0, maxChance);
		int compareChanceHigh = 0;
		for (int i = 0; i < lootItems.Count; i++) {
			int compareChanceLow = compareChanceHigh;
			compareChanceHigh += lootItems[i].chanceMultiplier;

			if (chance >= compareChanceLow && chance < compareChanceHigh) {
				itemToLoot = lootItems[i];
				break;
			}
		}

		itemQuantity = Random.Range (itemToLoot.minQuantity, itemToLoot.maxQuantity + 1);
	}

	public void LootItems () {
		ApplyItems ();
		DisplayItems ();

		hasBeenLooted = true;
		lootableIndicator.SetActive (false);
		unlootableIndicator.SetActive (true);
	}

	void ApplyItems () {
		switch (itemToLoot.type) {
		case LootItem.ItemType.Nothing:
			break;
		case LootItem.ItemType.Food:
			GameObject.FindObjectOfType<PlayerHealth>().Heal(itemQuantity);
			break;
		case LootItem.ItemType.Pistol:
			WeaponStorage.Instance.Ammo["Pistol"] += itemQuantity;
			Debug.Log ("get bullets");
			break;
		case LootItem.ItemType.Shotgun:
			WeaponStorage.Instance.Ammo["Shotgun"] += itemQuantity;
			break;
		case LootItem.ItemType.Zombie:
			// TODO
			break;
		}
	}

	void DisplayItems () {
		string title = "";
		string info = "";

		switch (itemToLoot.type) {
		case LootItem.ItemType.Nothing:
			title = "NOTHING";
			info = "it was empty!";
			break;
		case LootItem.ItemType.Food:
			title = "FOOD x " + itemQuantity;
			info = "+ " + itemQuantity + " health";
			break;
		case LootItem.ItemType.Pistol:
			title = "PISTOL";
			info = "+ " + itemQuantity + " bullets";
			break;
		case LootItem.ItemType.Shotgun:
			title = "SHOTGUN";
			info = "+ " + itemQuantity + " shells";
			break;
		case LootItem.ItemType.Zombie:
			title = "ZOMBIE";
			info = "+ he's angry!";
			break;
		}

		PlayerLooter.UpdateUIText (title, info);
		Debug.Log (title + "\n" + info);
	}

	void OnDrawGizmos () {
		Gizmos.color = Color.grey;
		Gizmos.DrawWireSphere (transform.position, clearRadius);
	}
}
