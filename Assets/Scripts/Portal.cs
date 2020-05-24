/*
This is the script for the portal. 
Once the portal is activated, standing on it will end the level. 
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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
                if ((SceneManager.GetActiveScene().name != "TutorialLevel") && (player.GetComponent<PlayerController>().health == 20))
                {
                    // warning: this assumes perfect health is 20!!!
                    LevelTracker.starsCollected++;
                    winText.text =  "*** Perfect Health - Bonus Star Earned ***" +
                                    "\nLevel Complete!\nStars collected: " + LevelTracker.starsCollected.ToString() + 
                                    "\nEnemies Defeated: " + LevelTracker.enemiesDefeated.ToString() + 
                                    "\nPress ENTER to continue\nPress BACKSPACE replay level\nPress TAB to return home";
                }
                else
                {
                    winText.text = "Level Complete!\nStars collected: " + LevelTracker.starsCollected.ToString() + 
                                    "\nEnemies Defeated: " + LevelTracker.enemiesDefeated.ToString() + 
                                    "\nPress ENTER to continue\nPress BACKSPACE replay level\nPress TAB to return home";
                }
                player.GetComponent<PlayerController>().FinishLevel();
            }
        }
    }
}
