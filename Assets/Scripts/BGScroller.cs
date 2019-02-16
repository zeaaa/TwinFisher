using System.Collections;
using UnityEngine;
using System.Collections;

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
		if (transform.position.z < Camera.main.transform.position.z-length)
			transform.position += Vector3.forward * time*length;

	}
}