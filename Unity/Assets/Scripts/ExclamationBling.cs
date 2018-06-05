using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExclamationBling : MonoBehaviour {
	[SerializeField] float animationRate;
	[SerializeField] AnimationCurve scaleCurve;

	float timer;
	Vector3 defaultScale;

	void Awake () {
		defaultScale = transform.localScale;
	}

	void Update () {
		transform.localScale = defaultScale * (1f + scaleCurve.Evaluate (timer));
		timer += Time.deltaTime * animationRate;
	}

	void OnEnable () {
		timer = 0f;
	}
}
