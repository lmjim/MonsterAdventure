using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSwitch : MonoBehaviour
{   
    void Start()
    {
        if(PlayerSelection.fiery)
        {
            transform.GetChild(0).gameObject.SetActive(true); // fiery
            transform.GetChild(1).gameObject.SetActive(false); // cyclops
        }
        else
        {
            transform.GetChild(0).gameObject.SetActive(false); // fiery
            transform.GetChild(1).gameObject.SetActive(true); // cyclops
        }
    }

    public static GameObject DefinePlayer(GameObject players)
    {
        GameObject player;
        if(PlayerSelection.fiery)
        {
            player = players.transform.GetChild(0).gameObject;
        }
        else
        {
            player = players.transform.GetChild(1).gameObject;
        }
        return player;
    }
}
