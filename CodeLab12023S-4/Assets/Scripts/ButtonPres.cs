using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class ButtonPres : MonoBehaviour
{
    private string playerName;
    public static string nickName;

    public TextMeshProUGUI recordName;
    public GameObject train;
    public GameObject station;

    public void SubmitName()//on submission button clicked 
    {
        playerName = recordName.text;//get the input text 
        string[] nameParts = playerName.Split(' ');//split between parts
        nickName = nameParts[0];//take only the first part

        GameObject[] destroyOnGS = GameObject.FindGameObjectsWithTag("GSDestroy");
        foreach (GameObject GS in destroyOnGS)
        {
            GameObject.Destroy(GS);
        }

        train.SetActive(true);
        station.SetActive(true);
    }
}
