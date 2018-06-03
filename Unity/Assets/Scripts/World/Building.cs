using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour {

	public int clearRadius;
	public float minScale;
	public float maxScale;

	[SerializeField] Collider doorCollider;

	void OnDrawGizmos () {
		Gizmos.color = Color.grey;
		Gizmos.DrawWireSphere (transform.position, clearRadius);
	}
}
