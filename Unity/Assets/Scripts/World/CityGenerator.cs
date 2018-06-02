using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class CityGenerator : MonoBehaviour {

	[System.Serializable] struct Zone {
		public string name;
		public Color uiColour;
		public int radius;
		public int numBuildings;
		public float angleIncrement;
		public List<Building> buildings;
	}

	[SerializeField] List<Zone> zones;

	void Awake () {
		Generate ();
	}

	void Generate () {
		int nZones = zones.Count;
		int startRadius = 0;
		int endRadius = 0;
		for (int z = 0; z < nZones; z++) {
			Zone zone = zones[z];
			endRadius += zone.radius;

			for (int b = 0; b < zone.numBuildings; b++) {
				bool isAbleToBuild = false;
				Vector3 buildPos = Vector3.zero;
				while (isAbleToBuild == false) {
					float radialDist = Random.Range (startRadius, endRadius);
					float angle = Random.Range (0f, 360f);

					buildPos = Quaternion.Euler (0f, angle, 0f) * new Vector3 (0f, 0f, radialDist);

					// TODO: don't spawn stuff too close together, ta
					isAbleToBuild = true;
				}

				CreateBuilding (buildPos, zone);
			}

			startRadius += zone.radius;
		}
	}

	void CreateBuilding (Vector3 position, Zone zone) {
		if (zone.buildings.Count == 0) {
			return; // abort abort
		}

		float facing = Random.Range (0f, 360f);
		facing = facing - facing % zone.angleIncrement;
			
		Building toInstantiate = zone.buildings[Random.Range (0, zone.buildings.Count)];
		Instantiate (toInstantiate, position, Quaternion.Euler (0f, facing, 0f));
	}

	void OnDrawGizmos () {
		int nZones = zones.Count;
		int accRadius = 0;
		for (int i = 0; i < nZones; i++) {
			Zone zone = zones[i];
			accRadius += zone.radius;

			Gizmos.color = zone.uiColour;
			Gizmos.DrawWireSphere (Vector3.zero, accRadius);
		}
	}

	/* this needs to be implemented in an editor, but what the heck we don't need it anyway
	void OnSceneGUI () {
		int nzones = zones.Count;
		for (int i = 0; i < nzones; i++) {
			Zone zone = zones[i];

			Handles.Label (new Vector3 (0f, 0.5f, zone.radius), zone.name);
			Handles.Label (new Vector3 (0f, 0.5f, -zone.radius), zone.name);
			Handles.Label (new Vector3 (zone.radius, 0.5f, 0f), zone.name);
			Handles.Label (new Vector3 (-zone.radius, 0.5f, 0f), zone.name);
		}
	}*/
}