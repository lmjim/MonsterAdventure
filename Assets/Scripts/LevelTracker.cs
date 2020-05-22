/*
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
    }

    void Update()
    {
        if(levelOver)
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                SceneManager.LoadScene("Home", LoadSceneMode.Single);
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
                    string level = SceneManager.GetActiveScene().name;
                    switch (level)
                    {
                        case "TutorialLevel":
                            SceneManager.LoadScene("Level1", LoadSceneMode.Single);
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
