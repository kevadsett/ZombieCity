using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLooter : MonoBehaviour {
	[SerializeField] KeyCode lootButton;
	[SerializeField] float maxLootRadius;

	void Update () {
		if (Input.GetKeyDown (lootButton)) {
			
		}
	}
}