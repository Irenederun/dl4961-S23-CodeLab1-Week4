
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOver : MonoBehaviour
{
    public static bool gameOver = false;//initialize boolean; make it static so accessible from GM script

    private void OnTriggerEnter2D(Collider2D col)//when train head enters car trigger 
    {
        if (col.name == "TrainHead")//check for specific collider 
        {
            Debug.Log("GameOver!");//debug
            gameOver = true;//set boolean to true; trigger game over in GM script.
        }
    }
}
