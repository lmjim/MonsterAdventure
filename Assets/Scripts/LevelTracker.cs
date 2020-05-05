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

    public static bool bossPassed = false;
    public static int  bossMinions = 0;

    public static bool levelOver = false;
    public static int starsCollected = 0;
    public static int enemiesDefeated = 0;

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
            if (Input.GetKeyDown(KeyCode.Return))
            {
                SceneManager.LoadScene("Home", LoadSceneMode.Single);
                starsCollected = 0;
                enemiesDefeated = 0;
                levelOver = false;
            }
        }
    }

    public static void EndLevel(bool alive)
    {
        levelOver = true;
        if(alive)
        {
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
                    break;
                case "Level2":
                    level2Passed = alive;
                    level2Stars = Math.Max(level2Stars, starsCollected);
                    level2Enemies = Math.Max(level2Enemies, enemiesDefeated);
                    break;
                case "Level3":
                    level3Passed = alive;
                    level3Stars = Math.Max(level3Stars, starsCollected);
                    level3Enemies = Math.Max(level3Enemies, enemiesDefeated);
                    break;
                case "BossLevel":
                    bossPassed = alive;
                    bossMinions = Math.Max(bossMinions, enemiesDefeated);
                    break;
            }
        }
    }
}
