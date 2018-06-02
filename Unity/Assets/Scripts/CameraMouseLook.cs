using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZombieCity
{
	public class CameraMouseLook : MonoBehaviour
	{
		public float Sensitivity = 5.0f;

		public float Smoothing = 2.0f;

		public bool Invert = true;

		private Vector2 mouseLook;
		private Vector2 smoothVector;

		private GameObject character;

		// Use this for initialization
		void Start()
		{
			character = transform.parent.gameObject;
		}

		// Update is called once per frame
		void Update()
		{
			var mouseDelta = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));
			mouseDelta = Vector2.Scale(mouseDelta, new Vector2(Sensitivity * Smoothing, Sensitivity * Smoothing));
			smoothVector.x = Mathf.Lerp(smoothVector.x, mouseDelta.x, 1f / Smoothing);
			smoothVector.y = Mathf.Lerp(smoothVector.y, mouseDelta.y, 1f / Smoothing);

			mouseLook += smoothVector;

			mouseLook.y = Mathf.Clamp(mouseLook.y, -85f, 85f);

			transform.localRotation = Quaternion.AngleAxis(Invert ? -mouseLook.y : mouseLook.y, Vector3.right);
			character.transform.localRotation = Quaternion.AngleAxis(mouseLook.x, character.transform.up);
		}
	}
}