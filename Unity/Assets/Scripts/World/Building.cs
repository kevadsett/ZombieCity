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
		Debug.LogWarning ("LOOTING ITEMS!");
		hasBeenLooted = true;
	}

	void OnDrawGizmos () {
		Gizmos.color = Color.grey;
		Gizmos.DrawWireSphere (transform.position, clearRadius);
	}
}
