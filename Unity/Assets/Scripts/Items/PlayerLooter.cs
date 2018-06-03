using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLooter : MonoBehaviour {
	[SerializeField] KeyCode lootButton;
	[SerializeField] float maxLootRadius;
	[SerializeField] GameObject pressFireToLootText;
	[SerializeField] GameObject alreadyLootedText;

	void Update () {
		Vector3 targetDir = Camera.main.transform.forward;
		Vector3 sourcePos = Camera.main.transform.position;

		RaycastHit hit;
		if (Physics.Raycast(sourcePos, targetDir, out hit))
		{
			pressFireToLootText.SetActive (false);
			alreadyLootedText.SetActive (false);

			for (int i = 0; i < CityGenerator.numBuildings; i++) {
				Building building = CityGenerator.GetBuilding (i);

				Collider doorCollider = building.doorCollider;

				if (hit.collider == doorCollider && Vector3.Distance (doorCollider.transform.position, sourcePos) < maxLootRadius) {

					if (building.hasBeenLooted) {
						alreadyLootedText.SetActive (true);
					} else {
						pressFireToLootText.SetActive (true);
					}

					if (Input.GetKeyDown (lootButton) && building.hasBeenLooted == false) {
						Debug.LogWarning ("LOOTING SHOULD HAPPEN NOW");
						building.hasBeenLooted = true;
					}
				}
			}
		}
	}
}