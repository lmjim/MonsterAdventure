/*
This is the script for the portal. 
Once the portal is activated, standing on it will end the level. 
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Portal : MonoBehaviour
{
    public GameObject players;
    public Text winText;
    
    private GameObject player;
    private ParticleSystem ps;   

    void Start()
    {
        player = PlayerSwitch.DefinePlayer(players);

        ps = transform.GetChild(0).gameObject.transform.GetChild(0).GetComponent<ParticleSystem>();
        winText.text = "";
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player")) // when the player steps on into the portal
        {
            if(ps.isEmitting)
            {
                winText.text = "Level Complete!\nStars collected: " + LevelTracker.starsCollected.ToString() + "\nEnemies Defeated: " + LevelTracker.enemiesDefeated.ToString() + "\nPress Enter to return home";
                player.GetComponent<PlayerController>().FinishLevel();
            }
        }
    }
}
