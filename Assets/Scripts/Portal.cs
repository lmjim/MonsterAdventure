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
    public GameObject dimmer;
    public GameObject starUI;
    
    private GameObject player;
    private ParticleSystem ps;

    private AudioSource audioSource;
    public AudioClip win;
    public static bool onButton = false; // Might remove later

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        player = PlayerSwitch.DefinePlayer(players);

        ps = transform.GetChild(0).gameObject.transform.GetChild(0).GetComponent<ParticleSystem>();
        winText.text = "";

        checkStar(); // if bonus star has been earned before, show it on UI
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
                    starUI.SetActive(true);
                    LevelTracker.star5 = true;
                    LevelTracker.countStars();
                    dimmer.SetActive(true);
                    winText.text =  "*** Perfect Health - Bonus Star Earned ***" +
                                    "\nLevel Complete!\nStars collected: " + LevelTracker.starsCollected.ToString() + 
                                    "\nEnemies Defeated: " + LevelTracker.enemiesDefeated.ToString() + 
                                    "\nPress ENTER to continue\nPress BACKSPACE replay level\nPress TAB to return home";
                    PlayWinSound();
                }
                else
                {
                    LevelTracker.countStars();
                    dimmer.SetActive(true);
                    winText.text = "Level Complete!\nStars collected: " + LevelTracker.starsCollected.ToString() + 
                                    "\nEnemies Defeated: " + LevelTracker.enemiesDefeated.ToString() + 
                                    "\nPress ENTER to continue\nPress BACKSPACE replay level\nPress TAB to return home";
                    PlayWinSound();
                }
                player.GetComponent<PlayerController>().FinishLevel();
            }
        }
    }

    public void PlayWinSound() // Sound fx when reach portal
    {
        audioSource.PlayOneShot(win);
        onButton = true;
    }

    void checkStar()
    {
        string level = SceneManager.GetActiveScene().name;
        switch (level)
        {
            case "Level1":
                if (LevelTracker.level1Star5) {LevelTracker.star5 = true; starUI.SetActive(true);}
                break;
            case "Level2":
                if (LevelTracker.level2Star5) {LevelTracker.star5 = true; starUI.SetActive(true);}
                break;
            case "Level3":
                if (LevelTracker.level3Star5) {LevelTracker.star5 = true; starUI.SetActive(true);}
                break;
        }
    }
}
