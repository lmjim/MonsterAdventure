using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelInfo : MonoBehaviour
{
    public int totalStars;
    public int totalEnemies;
    public int level;
    
    private TextMeshPro info;

    void Start()
    {
        info = GetComponent<TextMeshPro>();
        switch (level)
        {
            case 0: // tutorial level
                info.text = "Stars Collected:\n" + LevelTracker.tutorialStars.ToString() + "/" + totalStars.ToString() + "\nEnemies Defeated:\n" + LevelTracker.tutorialEnemies.ToString() + "/" + totalEnemies.ToString();
                break;
            case 1: // level 1
                info.text = "Stars Collected:\n" + LevelTracker.level1Stars.ToString() + "/" + totalStars.ToString() + "\nEnemies Defeated:\n" + LevelTracker.level1Enemies.ToString() + "/" + totalEnemies.ToString();
                break;
            case 2: // level 2
                info.text = "Stars Collected:\n" + LevelTracker.level2Stars.ToString() + "/" + totalStars.ToString() + "\nEnemies Defeated:\n" + LevelTracker.level2Enemies.ToString() + "/" + totalEnemies.ToString();
                break;
            case 3: // level 3
                info.text = "Stars Collected:\n" + LevelTracker.level3Stars.ToString() + "/" + totalStars.ToString() + "\nEnemies Defeated:\n" + LevelTracker.level3Enemies.ToString() + "/" + totalEnemies.ToString();
                break;
            case 4: // boss level
                if(LevelTracker.bossPassed)
                {
                    info.text = "Boss Defeated:\nYes\nMinions Defeated:\n" + LevelTracker.bossMinions.ToString() + "/" + totalEnemies.ToString();
                }
                else
                {
                    info.text = "Boss Defeated:\nNo\nMinions Defeated:\n" + LevelTracker.bossMinions.ToString() + "/" + totalEnemies.ToString();
                }
                break;
        }
    }
}
