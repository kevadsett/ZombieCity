using UnityEngine;

public class BulletMovement : MonoBehaviour
{
	public float Speed;

	public Vector3 TargetPosition;

	void Update()
	{
		var step = Speed * Time.deltaTime;
		Debug.Log(TargetPosition);
		transform.position = Vector3.MoveTowards(transform.position, TargetPosition, step);

		if (Vector3.Magnitude(transform.position - TargetPosition) < 0.1f)
		{
			Destroy(gameObject);
		}
	}
}
