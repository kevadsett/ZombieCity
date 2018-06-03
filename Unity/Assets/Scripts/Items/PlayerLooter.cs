using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLooter : MonoBehaviour {
	[SerializeField] KeyCode lootButton;
	[SerializeField] float maxLootRadius;

	public static bool showCantLootText { get; private set; }
	public static bool showCanLootText { get; private set; }
	public static bool showLootUI { get; private set; }

	void Update () {
		if (showLootUI) {
			HandleLootUI ();
		} else {
			TestForLooting ();
		}
	}

	void HandleLootUI () {
		if (Input.GetKeyDown (lootButton)) {
			showLootUI = false;
		}
	}

	void TestForLooting () {
		Vector3 targetDir = Camera.main.transform.forward;
		Vector3 sourcePos = Camera.main.transform.position;

		RaycastHit hit;
		if (Physics.Raycast(sourcePos, targetDir, out hit))
		{
			showCanLootText = false;
			showCantLootText = false;

			for (int i = 0; i < CityGenerator.numBuildings; i++) {
				Building building = CityGenerator.GetBuilding (i);

				Collider doorCollider = building.doorCollider;

				if (hit.collider == doorCollider && Vector3.Distance (doorCollider.transform.position, sourcePos) < maxLootRadius) {

					if (building.hasBeenLooted) {
						showCantLootText = true;
					} else {
						showCanLootText = true;
					}

					if (Input.GetKeyDown (lootButton) && building.hasBeenLooted == false) {
						Debug.LogWarning ("LOOTING SHOULD HAPPEN NOW");

						showLootUI = true;
						building.hasBeenLooted = true;
					}
				}
			}
		}
	}
}