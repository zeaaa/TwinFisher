using UnityEngine;

// Include the namespace required to use Unity UI
using UnityEngine.UI;
using System.Collections;

public class PlayerController : MonoBehaviour
{

    // Create public variables for player speed, and for the Text UI game objects
    public float speed;
    public Text countText;
    public Text winText;
    public Transform targetW;
    public Transform targetS;
    public Transform targetA;
    public Transform targetD;



    // Create private references to the rigidbody component on the player, and the count of pick up objects picked up so far
    private Rigidbody rb;
    private int count;
    private bool W = false;
    private bool S = false;
    private bool A = false;
    private bool D = false;
    private float time = 0.0f;
    Vector3 m_EulerAngleVelocity;
    void Start()
    {
        // Assign the Rigidbody component to our private rb variable
        rb = GetComponent<Rigidbody>();

        // Set the count to zero 
        count = 0;

        // Run the SetCountText function to update the UI (see below)
        SetCountText();

        // Set the text property of our Win Text UI to an empty string, making the 'You Win' (game over message) blank
        winText.text = "";

        time = 0.0f;
    }

    // Each physics step..
    void FixedUpdate()
    {
        time += Time.deltaTime;
        Debug.Log(time);
        Debug.Log(W);


        if (time > 3.0f)
        {
            W = false;
        }

        if (Input.GetKeyDown("space"))
        {

            Vector3 movementV = new Vector3(0.0f, 2f, 0.0f);
            rb.AddForce(movementV * speed);


        }
        if (Input.GetKeyDown("w"))
        {
            if (W == false)
            {
                rb.Sleep();
                rb.WakeUp();
                Vector3 direction = targetW.position - transform.position;
                Quaternion rotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Lerp(transform.rotation, rotation, 10000000.0f);
                Vector3 movementV = new Vector3(0.0f, 0.0f, 10.0f);
                rb.AddForce(movementV * speed * 2);
            }

        }

        if (Input.GetKeyDown("s"))
        {
            if (W == false)
            {
                rb.Sleep();
                rb.WakeUp();
                Vector3 direction = targetS.position - transform.position;
                Quaternion rotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Lerp(transform.rotation, rotation, 10000000.0f);
                rb.transform.rotation = Quaternion.RotateTowards(Quaternion.AngleAxis(0.0f, new Vector3(0.0f, 0.0f, 0.0f)), Quaternion.AngleAxis(90.0f, new Vector3(0.0f, 90.0f, 0.0f)), 360);
                Vector3 movementV = new Vector3(0.0f, 0.0f, -10.0f);
                rb.AddForce(movementV * speed * 2);


            }



        }

        if (Input.GetKeyDown("a"))
        {
            if (W == false)
            {
                rb.Sleep();
                rb.WakeUp();
                Vector3 direction = targetA.position - transform.position;
                Quaternion rotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Lerp(transform.rotation, rotation, 10000000.0f);
                rb.transform.rotation = Quaternion.RotateTowards(Quaternion.AngleAxis(0.0f, new Vector3(0.0f, 0.0f, 0.0f)), Quaternion.AngleAxis(90.0f, new Vector3(0.0f, 90.0f, 0.0f)), 360);
                Vector3 movementV = new Vector3(-10.0f, 0.0f, 0.0f);
                rb.AddForce(movementV * speed * 2);

            }
        }

        if (Input.GetKeyDown("d"))
        {
            if (W == false)
            {
                rb.Sleep();
                rb.WakeUp();
                Vector3 direction = targetD.position - transform.position;
                Quaternion rotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Lerp(transform.rotation, rotation, 10000000.0f);
                rb.transform.rotation = Quaternion.RotateTowards(Quaternion.AngleAxis(0.0f, new Vector3(0.0f, 0.0f, 0.0f)), Quaternion.AngleAxis(90.0f, new Vector3(0.0f, 90.0f, 0.0f)), 360);
                Vector3 movementV = new Vector3(10.0f, 0.0f, 0.0f);
                rb.AddForce(movementV * speed * 2);
            }


        }

    }

    // When this game object intersects a collider with 'is trigger' checked, 
    // store a reference to that collider in a variable named 'other'..
    void OnTriggerEnter(Collider other)
    {
        // ..and if the game object we intersect has the tag 'Pick Up' assigned to it..
        if (other.gameObject.CompareTag("Pick Up"))
        {
            // Make the other game object (the pick up) inactive, to make it disappear
            other.gameObject.SetActive(false);

            // Add one to the score variable 'count'
            count = count + 1;

            // Run the 'SetCountText()' function (see below)
            SetCountText();
        }
    }

    void OnCollisionEnter(Collision wall)
    {
        // ..and if the game object we intersect has the tag 'Pick Up' assigned to it..
        if (wall.gameObject.CompareTag("wall"))
        {
            Debug.Log("wall");
            Debug.Log(W);
            time = 0.0f;
            W = true;

        }

    }

    // Create a standalone function that can update the 'countText' UI and check if the required amount to win has been achieved
    void SetCountText()
    {
        // Update the text field of our 'countText' variable
        countText.text = "Count: " + count.ToString();

        // Check if our 'count' is equal to or exceeded 12
        if (count >= 12)
        {
            // Set the text value of our 'winText'
            winText.text = "You Win!";
        }
    }
}