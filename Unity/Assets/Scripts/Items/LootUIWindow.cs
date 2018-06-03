using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;
using UnityEngine.UI;

public class LootUIWindow : MonoBehaviour {

	[SerializeField] GameObject disableParent;
	[SerializeField] Text titleText;
	[SerializeField] Text infoText;

	void Update () {
		GameObject.FindObjectOfType<FirstPersonController> ().enabled = !PlayerLooter.showLootUI;
		disableParent.SetActive (PlayerLooter.showLootUI);

		titleText.text = PlayerLooter.titleText;
		infoText.text = PlayerLooter.infoText;
	}
}
