using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour {

	public float clearRadius;
	public float minScale;
	public float maxScale;

	public Collider doorCollider;

	public bool hasBeenLooted { get; private set; }

	[System.Serializable] struct LootItem {
		public enum ItemType {
			Nothing, Food, Zombie, Pistol, Shotgun
		}

		public ItemType type;
		public int chanceMultiplier;
		public int minQuantity;
		public int maxQuantity;
	}

	[SerializeField] List<LootItem> lootItems;

	public void LootItems () {
		int maxChance = 0;
		for (int i = 0; i < lootItems.Count; i++) {
			maxChance += lootItems[i].chanceMultiplier;
		}

		LootItem lootedItem = lootItems[0];

		int chance = Random.Range (0, maxChance);
		int compareChanceHigh = 0;
		for (int i = 0; i < lootItems.Count; i++) {
			int compareChanceLow = compareChanceHigh;
			compareChanceHigh += lootItems[i].chanceMultiplier;

			if (chance >= compareChanceLow && chance < compareChanceHigh) {
				lootedItem = lootItems[i];
				break;
			}
		}

		int quantity = Random.Range (lootedItem.minQuantity, lootedItem.maxQuantity + 1);

		DisplayItems (lootedItem.type, quantity);

		hasBeenLooted = true;
	}

	void DisplayItems (LootItem.ItemType type, int quantity) {
		string title = "";
		string info = "";

		switch (type) {
		case LootItem.ItemType.Nothing:
			title = "NOTHING";
			info = "it was empty!";
			break;
		case LootItem.ItemType.Food:
			title = "FOOD";
			info = "+ " + quantity + " health";
			break;
		case LootItem.ItemType.Pistol:
			title = "PISTOL";
			info = "+ " + quantity + " bullets";
			break;
		case LootItem.ItemType.Shotgun:
			title = "SHOTGUN";
			info = "+ " + quantity + " shells";
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
