using System.Collections;
using UnityEngine;

public class BGScroller : MonoBehaviour
{
    public static int currentID = 0;
    public static int nextID = 1;
    private float scrollSpeed;
	public float length;
    public int id;
    Transform wharf;

    private const int totalCount = 2;
	void Start ()
	{
        wharf = transform.Find("Dock");
        scrollSpeed = -GameManager.Speed;
    }

	void FixedUpdate ()
	{
		transform.position += Vector3.forward * scrollSpeed;
        if (transform.position.z < 0 - length)
        {
            transform.position += Vector3.forward * length * 2;
            currentID++;
            if (currentID > (totalCount - 1))
                currentID -= totalCount;
            nextID = currentID + 1;
            if (nextID > (totalCount - 1))
                nextID -= totalCount;
            OpenWharf(false);

        }

	}

    public void OpenWharf(bool status) {
        wharf.GetComponent<Wharf>().Open(status);
    }
}