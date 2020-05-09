using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelButtonSwitch : MonoBehaviour
{
    public int level;

    void Start()
    {
        switch (level)
        { // tutorial and level 1 always unlocked
            case 2: // level 2
                if(LevelTracker.level1Passed)
                {
                    transform.GetChild(1).gameObject.transform.GetChild(0).gameObject.SetActive(false); // turn off "locked" button
                    transform.GetChild(2).gameObject.transform.GetChild(0).gameObject.SetActive(true); // turn on "unlocked" button                    
                }
                break;
            case 3: // level 3
                if(LevelTracker.level2Passed)
                {
                    transform.GetChild(1).gameObject.transform.GetChild(0).gameObject.SetActive(false); // turn off "locked" button
                    transform.GetChild(2).gameObject.transform.GetChild(0).gameObject.SetActive(true); // turn on "unlocked" button  
                }
                break;
            case 4: // boss level
                if(LevelTracker.level3Passed)
                {
                    transform.GetChild(1).gameObject.transform.GetChild(0).gameObject.SetActive(false); // turn off "locked" button
                    transform.GetChild(2).gameObject.transform.GetChild(0).gameObject.SetActive(true); // turn on "unlocked" button  
                }
                break;
        }
    }
}
