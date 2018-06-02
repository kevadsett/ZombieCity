using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour {

	public int clearRadius;

	void OnDrawGizmos () {
		Gizmos.color = Color.grey;
		Gizmos.DrawWireSphere (transform.position, clearRadius);
	}
}
