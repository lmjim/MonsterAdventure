﻿/*
References:
https://www.sitepoint.com/saving-data-between-scenes-in-unity/
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class LevelTracker : MonoBehaviour
{
    public static LevelTracker Instance;

    public static int  tutorialStars = 0;
    public static int  tutorialEnemies = 0;
    public static bool tutorialPassed = false;
    
    public static bool level1Passed = false;
    public static int  level1Stars = 0;
    public static int  level1Enemies = 0;
    
    public static bool level2Passed = false;
    public static int  level2Stars = 0;
    public static int  level2Enemies = 0;
    
    public static bool level3Passed = false;
    public static int  level3Stars = 0;
    public static int  level3Enemies = 0;

    public static bool levelOver = false;
    public static int starsCollected = 0;
    public static int enemiesDefeated = 0;

    public static bool learnedSprint = false;
    public static bool learnedDoubleJump = false;
    public static bool learnedWallJump = false;

    private static bool success = false;

    public static bool onHome = false;
    public static bool onLevel1 = false;

    // This is the background music that gets played on home scene
    // if you don't go consecutively (like playing a level and presisng
    // tab to return home)
    AudioSource audioSource; 

    void Awake ()   
    {
        if (Instance == null)
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy (gameObject);
        }

        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        // If the active scene is anything other than home,
        // stop playing the background music
        if (!(SceneManager.GetActiveScene().name == "Home"))
        {
            audioSource.Stop();  
        }

        if (levelOver)
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                SceneManager.LoadScene("Home", LoadSceneMode.Single);

                // Unless current scene is tutorial, begin playing the background music
                if (!(SceneManager.GetActiveScene().name == "TutorialLevel") || !(SceneManager.GetActiveScene().name == "TutorialLevel"))
                {
                    audioSource.Play();
                }

                ResetVariables();
            }
            
           
            if (Input.GetKeyDown(KeyCode.Backspace))
            {
                string level = SceneManager.GetActiveScene().name;
                SceneManager.LoadScene(level, LoadSceneMode.Single);
                ResetVariables();
            }
            if (success)
            {
                if (Input.GetKeyDown(KeyCode.Return))
                {
                    // Not sure if this is actually doing anything
                    // but keep for now
                    //audioSource.Stop();

                    string level = SceneManager.GetActiveScene().name;
                    switch (level)
                    {
                        case "TutorialLevel":
                            SceneManager.LoadScene("Level1", LoadSceneMode.Single);
                            onLevel1 = true; // For BackgroundMusic script
                            break;
                        case "Level1":
                            SceneManager.LoadScene("Level2", LoadSceneMode.Single);
                            break;
                        case "Level2":
                            SceneManager.LoadScene("Level3", LoadSceneMode.Single);
                            break;
                        case "Level3":
                            SceneManager.LoadScene("Home", LoadSceneMode.Single);
                            break;
                    }
                    ResetVariables();
                }
            }

            
        }
    }

    public static void EndLevel(bool alive)
    {
        levelOver = true;
        if(alive)
        {
            success = true;
            string level = SceneManager.GetActiveScene().name;
            switch (level)
            {
                case "TutorialLevel":
                    tutorialPassed = alive;
                    tutorialStars = Math.Max(tutorialStars, starsCollected);
                    tutorialEnemies = Math.Max(tutorialEnemies, enemiesDefeated);
                    break;
                case "Level1":
                    level1Passed = alive;
                    level1Stars = Math.Max(level1Stars, starsCollected);
                    level1Enemies = Math.Max(level1Enemies, enemiesDefeated);
                    learnedSprint = true;
                    break;
                case "Level2":
                    level2Passed = alive;
                    level2Stars = Math.Max(level2Stars, starsCollected);
                    level2Enemies = Math.Max(level2Enemies, enemiesDefeated);
                    learnedDoubleJump = true;
                    break;
                case "Level3":
                    level3Passed = alive;
                    level3Stars = Math.Max(level3Stars, starsCollected);
                    level3Enemies = Math.Max(level3Enemies, enemiesDefeated);
                    learnedWallJump = true;
                    break;
            }
        }
    }

    void ResetVariables()
    {
        starsCollected = 0;
        enemiesDefeated = 0;
        levelOver = false;
        success = false;
    }
}
