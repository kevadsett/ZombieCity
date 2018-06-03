using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class LootUIWindow : MonoBehaviour {

	[SerializeField] GameObject disableParent;

	void Update () {
		GameObject.FindObjectOfType<FirstPersonController> ().enabled = !PlayerLooter.showLootUI;
		disableParent.SetActive (PlayerLooter.showLootUI);
	}
}
