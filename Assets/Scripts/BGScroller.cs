using System.Collections;
using UnityEngine;

public class BGScroller : MonoBehaviour
{
    private float scrollSpeed;
	public float length;
	public float time;
	private Vector3 startPosition;

	void Start ()
	{
		startPosition = transform.position;
        scrollSpeed = -GameManager.Speed;
    }

	void FixedUpdate ()
	{

		transform.position += Vector3.forward * scrollSpeed;
		if (transform.position.z < 0 -length)
			transform.position += Vector3.forward * time*length;

	}
}