using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZombieCity
{
	public class CharacterController : MonoBehaviour
	{
		public float Speed = 10.0f;

		// Use this for initialization
		void Start ()
		{
			Cursor.lockState = CursorLockMode.Locked;
		}

		// Update is called once per frame
		void Update ()
		{
			var translation = Input.GetAxis("Vertical") * Speed;
			var strafe = Input.GetAxis("Horizontal") * Speed;
			translation *= Time.deltaTime;
			strafe *= Time.deltaTime;

			transform.Translate(strafe, 0, translation);

			// let us get our mouse back!
			if (Input.GetKeyDown("escape"))
			{
				Cursor.lockState = CursorLockMode.None;
			}
		}
	}
}
