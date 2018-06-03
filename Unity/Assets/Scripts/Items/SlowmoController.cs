using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowmoController : MonoBehaviour {

	[SerializeField] float uiSlowMoSpeed;

	void Update () {
		Time.timeScale = (PlayerLooter.showLootUI) ? uiSlowMoSpeed : 1f;
	}
}