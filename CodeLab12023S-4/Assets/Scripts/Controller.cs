using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using UnityEditorInternal;
using UnityEngine;

public class Controller : MonoBehaviour
{
    public static Controller Instance;
    //make it a static

    Rigidbody2D rb2D;
    public float torque = 1;
    public float forceAmount = 5;
    public GameObject playerTrain;

    private void Awake()
    {
        if (Instance == null)
            //if no other instance of this game object in scene
        {
            DontDestroyOnLoad(gameObject);
            //do not destroy
            Instance = this;
            //mark that there is already an instance here
            playerTrain = gameObject;
            //set playerTrain to this game object
        }
        else
            //if there is one already 
        {
            Destroy(gameObject);
            //delete self
            playerTrain = Collision.Instance.lastTrain;
            //set player train to the last car inherited from the previous scene
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        float turn = Input.GetAxis("Horizontal");
        //get the axis of the object as a base for the torque
        
        if (Input.GetKey(KeyCode.W))
        {
            rb2D.velocity = transform.right * forceAmount;
            //used transform instead of vector2 because it'll move "forward" instead of simply upward 
            //rb2D.AddForce(Vector2.up * forceAmount);
            //rb2D.AddTorque(torque * turn);
        }

        if (Input.GetKey(KeyCode.A))
        {
            //rb2D.AddForce(Vector2.left * forceAmount);
            rb2D.AddTorque(torque * turn);
        }

        if (Input.GetKey(KeyCode.S))
        {
            rb2D.velocity = -transform.right * forceAmount;
            //rb2D.AddForce(Vector2.down * forceAmount);
            //rb2D.AddTorque(torque * turn);
        }

        if (Input.GetKey(KeyCode.D))
        {
            //rb2D.AddForce(Vector2.right * forceAmount);
            rb2D.AddTorque(torque * turn);
        }

        rb2D.velocity *= 0.99f;
        rb2D.angularVelocity *= 0.9f;
    }  
}
