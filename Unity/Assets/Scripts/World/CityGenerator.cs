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

	[SerializeField] ZombieAI zombiePrefab;
	[SerializeField] List<Zone> zones;

	List<Building> buildingsSpawned = new List<Building> ();

	public static CityGenerator Instance { get; set; }

	public static Building GetBuilding (int i) {
		return (Instance == null) ? null : Instance.buildingsSpawned[i];
	}

	public static int numBuildings { get {
			return (Instance == null) ? 0 : Instance.buildingsSpawned.Count;
		}
	}

	void Awake () {
		Instance = this;
	}

	public void Generate ()
	{
		buildingsSpawned.ForEach(building => Destroy(building.gameObject));
		buildingsSpawned.Clear();

		int nZones = zones.Count;
		int startRadius = 0;
		int endRadius = 0;
		for (int z = 0; z < nZones; z++) {
			Zone zone = zones[z];
			endRadius += zone.radius;

			for (int b = 0; b < zone.numBuildings; b++) {
				Building toInstantiate = zone.buildings[Random.Range (0, zone.buildings.Count)];

				bool isAbleToBuild = false;
				Vector3 buildPos = Vector3.zero;
				int attempts = 512;
				while (isAbleToBuild == false) {
					float radialDist = Random.Range (startRadius, endRadius);
					float angle = Random.Range (0f, 360f);

					buildPos = Quaternion.Euler (0f, angle, 0f) * new Vector3 (0f, 0f, radialDist);

					isAbleToBuild = true;
					for (int c = 0; c < buildingsSpawned.Count; c++) {
						Building testBiru = buildingsSpawned[c];
						Vector3 testPos = testBiru.transform.localPosition;

						float testRadius = testBiru.clearRadius + toInstantiate.clearRadius;
						float distBetween = Vector3.Distance (testPos, buildPos);

						// give up and place anyway after too many attempts
						if (distBetween < testRadius && attempts >= 0) {
							isAbleToBuild = false;
							break;
						}
					}

					attempts--;
				}

				if (isAbleToBuild) {
					CreateBuilding (buildPos, toInstantiate, zone);
				}
			}

			startRadius += zone.radius;
		}
	}

	void CreateBuilding (Vector3 position, Building toInstantiate, Zone zone) {
		if (zone.buildings.Count == 0) {
			return; // abort abort
		}

		float facing = Random.Range (0f, 360f);
		facing = facing - facing % zone.angleIncrement;

		Building instantiated = Instantiate (toInstantiate, position, Quaternion.Euler (0f, facing, 0f), transform);
		instantiated.transform.localScale = Vector3.one * Random.Range (instantiated.minScale, instantiated.maxScale);

		buildingsSpawned.Add (instantiated);

		SpawnZombies (instantiated);
	}

	void SpawnZombies (Building building) {
		float angle = Random.Range (0f, 360f);
		Vector3 spawnPos = Quaternion.Euler (0f, angle, 0f) * new Vector3 (0f, 0f, building.clearRadius) + building.transform.position
			+ Vector3.up * 2f; // TODO: wtf, this is weird

		Instantiate (zombiePrefab, spawnPos, Quaternion.Euler (0f, angle, 0f), building.transform);

		// TODO: spawn more than one (see design doc for info!)
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