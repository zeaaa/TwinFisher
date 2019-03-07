using UnityEngine;
using System.Collections;

public class Done_Mover : MonoBehaviour
{
	public float speed;
	public bool inPond=false;
	public Vector2 range;
	public float waitTime;
	float timer;
	bool waiting;
	Vector3 point;

	void Start ()
	{
		timer = 0;
		if(!inPond)
            GetComponent<Rigidbody>().velocity = transform.forward * speed;

	}

	void Update()
	{
		if (!inPond && transform.position.z < -10f)
			Destroy (gameObject);
		if (inPond) {
			if (waiting) {
				timer += Time.deltaTime;
				if (timer >= waitTime) {
					waiting = false;
					timer = 0;
					point.x = Random.Range (-range.x, range.x);
					point.z = Random.Range (-range.y, range.y);
				}
			} else {
				transform.position = Vector3.MoveTowards(transform.position, point , Time.deltaTime);
				if((transform.position-point).sqrMagnitude<0.01f)
					waiting = true;
			}
		}
	}
}
