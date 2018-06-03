using UnityEngine;

public class BulletMovement : MonoBehaviour
{
	public float Speed;

	public Vector3 TargetPosition;

	void Start()
	{
		var newDirection = Vector3.RotateTowards(transform.forward, transform.position - TargetPosition, Mathf.PI * 2, 0.0f);
		transform.rotation = Quaternion.LookRotation(newDirection);
	}

	void Update()
	{
		var step = Speed * Time.deltaTime;

		transform.position = Vector3.MoveTowards(transform.position, TargetPosition, step);

		if (Vector3.Magnitude(transform.position - TargetPosition) < 0.1f)
		{
			Destroy(gameObject);
		}
	}
}
