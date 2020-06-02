﻿/*
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
                    dimmer.SetActive(true);
                    winText.text =  "*** Perfect Health - Bonus Star Earned ***" +
                                    "\nLevel Complete!\nStars collected: " + LevelTracker.starsCollected.ToString() + 
                                    "\nEnemies Defeated: " + LevelTracker.enemiesDefeated.ToString() + 
                                    "\nPress ENTER to continue\nPress BACKSPACE replay level\nPress TAB to return home";
                    PlayWinSound();
                }
                else
                {
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
}
