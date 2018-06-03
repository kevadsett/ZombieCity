using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LootUICrosshair : MonoBehaviour {
	[SerializeField] bool showForLootAvailability;
	[SerializeField] Text uiTextObject;

	void Update () {
		if (showForLootAvailability) {
			uiTextObject.enabled = PlayerLooter.showCanLootText;
		} else {
			uiTextObject.enabled = PlayerLooter.showCantLootText;
		}
	}
}
