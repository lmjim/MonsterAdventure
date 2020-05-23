/*
 * References
 * https://www.youtube.com/watch?v=JKoBWBXVvKY
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroy : MonoBehaviour
{
    
    private void Awake()
    {
        GameObject[] obj = GameObject.FindGameObjectsWithTag("Music"); // Tagged background music with this, counts into array
        if (obj.Length > 1) // If second instance of music, destroy
        {
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(this.gameObject); // Run on particular game object to stay alive all the time
    
    }

    void Update()
    {
        if(LevelTracker.tutorialPassed)
        {
            Destroy(this.gameObject);
        }
    }
}
