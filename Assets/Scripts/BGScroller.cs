using System.Collections;
using UnityEngine;

public class BGScroller : MonoBehaviour
{
    public static int currentID = 0;
    public static int nextID = 1;
    [ShowOnly]
    private float scrollSpeed;
	public float length;
    public int id;
    Transform wharf;

    private const int totalCount = 3;

    private void Awake()
    {
        scrollSpeed = 0;
        wharf = transform.Find("Dock");
    }

    void Start ()
	{
        
    }

	void FixedUpdate ()
	{
		transform.position += Vector3.forward * -scrollSpeed;
        if (transform.position.z < 0 - length)
        {
            transform.position += Vector3.forward * length * totalCount;
            currentID++;
            if (currentID > (totalCount - 1))
                currentID -= totalCount;
            nextID = currentID + (totalCount - 1);
            if (nextID > (totalCount - 1))
                nextID -= totalCount;
            OpenWharf(false);
        }
	}



    public void OpenWharf(bool status) {
        wharf.GetComponent<Wharf>().Open(status);
    }

    public void SetSpeed(float speed)
    {
        scrollSpeed = speed;
    }


}