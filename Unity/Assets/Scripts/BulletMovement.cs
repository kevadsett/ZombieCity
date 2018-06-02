using UnityEngine;

public class BulletMovement : MonoBehaviour
{
	public float Speed;

	public float MaxDistBeforeDestroy;

	private Vector3 startPosition;

	void Start ()
	{
		startPosition = transform.position;
	}

	void Update()
	{
		var newPosition = transform.position + transform.forward * Speed * Time.deltaTime;
		transform.position = newPosition;

		if (Vector3.Magnitude(newPosition - startPosition) >= MaxDistBeforeDestroy)
		{
			Destroy(gameObject);
		}
	}
}
