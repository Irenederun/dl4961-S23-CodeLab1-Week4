using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Drawing;
using Unity.VisualScripting;
using UnityEditor.U2D.Path.GUIFramework;
using UnityEngine;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

public class Collision : MonoBehaviour
{
    public static Collision Instance;

    public GameObject lastTrain;
    private Rigidbody2D trainRb;
    private Vector3 trainPos;
    public GameObject car;
    private Rigidbody2D newTrainRb;
    
    private void Awake()
    {
        if (Instance == null)
            //if no other instance of this game object in scene
        {
            DontDestroyOnLoad(gameObject);
            //don't destroy
            Instance = this;
            //set instance to this game object
        }
        else
            //if there is
        {
            Destroy(gameObject);
            //destroy self
        }
    }
    
    // Start is called before the first frame update
    void Start()
    {
        lastTrain = Controller.Instance.playerTrain;
        //the last Train (to which the new car instantiates) at the beginning is the WASD controller Train
    }
    
    private void OnCollisionEnter2D(Collision2D col)
    //on collision with station
    {
        //Debug.Log("collided");

        if (col.gameObject.name == "Train")
            //if it's the player train 
        {
            trainRb = lastTrain.GetComponent<Rigidbody2D>();
            //get the rigid body
            trainPos = lastTrain.transform.position;
            //get the position

            transform.position = new Vector3(
                Random.Range(-6.6f,6.6f),
                Random.Range(-3.7f,3.7f));
            //move the station to another random position after collision

            GameObject trainCar = Instantiate(car);
            //instantiate from prefab
            trainCar.transform.position = trainPos + transform.right * 1.5f;
            //the position of the new car should be to the right to the last Train by 1.5
            
            //trainCar.transform.eulerAngles = Vector3.zero;
            
            trainCar.AddComponent<DistanceJoint2D>();
            //add distance joint 2D to the new car
            //but this still rotates around the last train. tried many of the 2D joints but didn't find one that works ideally
            DistanceJoint2D joint2D = trainCar.GetComponent<DistanceJoint2D>();
            //register it 
            joint2D.connectedBody = trainRb;
            //connect the joint to the last Train's rigid body

            if (lastTrain == Controller.Instance.playerTrain)
                //if it's the player train to which the new car is connected
            {
                joint2D.autoConfigureConnectedAnchor = false;
                joint2D.connectedAnchor = new Vector2(-2.4f, 0);
                //some refining stuff to make the distance reasonable
            }
            else
                //if it's one of the new cars
            {
                joint2D.autoConfigureDistance = false;
                joint2D.distance = 1.2f;
                //refine
            }

            lastTrain = trainCar;//reset last Train to this newly instantiated car
            GameManager.Instance.Score++;//add score
        }
    }
}
