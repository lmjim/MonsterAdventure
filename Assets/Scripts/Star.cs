/*
This is the script for the star particle.
When the star appears, the player should be able to collect.
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Star : MonoBehaviour
{
    public int starNumber;
    public GameObject starUI;

    private ParticleSystem ps;
    private bool collected = false;

    AudioSource audioSource;
    public AudioClip star;

    void Start()
    {
        ps = transform.GetChild(0).GetComponent<ParticleSystem>();
        audioSource = GetComponent<AudioSource>();

        checkStars(); // if star has been earned before, show it on UI
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player")) // when the player steps on the button
        {
            if(!collected & ps.isEmitting) // if the star hasn't already been collected and is visible
            {
                audioSource.PlayOneShot(star); // Sound fx when collecting star
                ps.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear); // collect the star
                collected = true; // can't collect it again
                starUI.SetActive(true);
                switch (starNumber)
                { // keep track of stars collected
                    case 1:
                        LevelTracker.star1 = true;
                        break;
                    case 2:
                        LevelTracker.star2 = true;
                        break;
                    case 3:
                        LevelTracker.star3 = true;
                        break;
                    case 4:
                        LevelTracker.star4 = true;
                        break;
                }
            }
        }
    }

    void checkStars()
    {
        string level = SceneManager.GetActiveScene().name;
        switch (starNumber)
        { // check which stars have been collected before
            case 1:
                switch (level)
                {
                    case "TutorialLevel":
                        if (LevelTracker.tutorialStar1) {LevelTracker.star1 = true; starUI.SetActive(true);}
                        break;
                    case "Level1":
                        if (LevelTracker.level1Star1) {LevelTracker.star1 = true; starUI.SetActive(true);}
                        break;
                    case "Level2":
                        if (LevelTracker.level2Star1) {LevelTracker.star1 = true; starUI.SetActive(true);}
                        break;
                    case "Level3":
                        if (LevelTracker.level3Star1) {LevelTracker.star1 = true; starUI.SetActive(true);}
                        break;
                }
                break;
            case 2:
                switch (level)
                {
                    case "Level1":
                        if (LevelTracker.level1Star2) {LevelTracker.star2 = true; starUI.SetActive(true);}
                        break;
                    case "Level2":
                        if (LevelTracker.level2Star2) {LevelTracker.star2 = true; starUI.SetActive(true);}
                        break;
                    case "Level3":
                        if (LevelTracker.level3Star2) {LevelTracker.star2 = true; starUI.SetActive(true);}
                        break;
                }
                break;
            case 3:
                switch (level)
                {
                    case "Level1":
                        if (LevelTracker.level1Star3) {LevelTracker.star3 = true; starUI.SetActive(true);}
                        break;
                    case "Level2":
                        if (LevelTracker.level2Star3) {LevelTracker.star3 = true; starUI.SetActive(true);}
                        break;
                    case "Level3":
                        if (LevelTracker.level3Star3) {LevelTracker.star3 = true; starUI.SetActive(true);}
                        break;
                }
                break;
            case 4:
                switch (level)
                {
                    case "Level1":
                        if (LevelTracker.level1Star4) {LevelTracker.star4 = true; starUI.SetActive(true);}
                        break;
                    case "Level2":
                        if (LevelTracker.level2Star4) {LevelTracker.star4 = true; starUI.SetActive(true);}
                        break;
                    case "Level3":
                        if (LevelTracker.level3Star4) {LevelTracker.star4 = true; starUI.SetActive(true);}
                        break;
                }
                break;
        }
    }

}
