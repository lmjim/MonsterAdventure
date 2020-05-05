using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelButton : MonoBehaviour
{
    public GameObject slime;
    public GameObject levelLabel;
    public GameObject levelInfo;
    public int level;

    private GameObject button;
    private bool onButton = false;

    void Start()
    {
        button = transform.GetChild(0).gameObject;
    }

    void Update()
    {
        if(onButton)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                switch (level)
                {
                    case 0: // tutorial level
                        SceneManager.LoadScene("TutorialLevel", LoadSceneMode.Single);
                        break;
                    case 1: // level 1
                        SceneManager.LoadScene("Level1", LoadSceneMode.Single);
                        break;
                    case 2: // level 2
                        if(LevelTracker.level1Passed)
                        {
                            SceneManager.LoadScene("Level2", LoadSceneMode.Single);
                        }
                        break;
                    case 3: // level 3
                        if(LevelTracker.level2Passed)
                        {
                            SceneManager.LoadScene("Level3", LoadSceneMode.Single);
                        }
                        break;
                    case 4: // boss level
                        if(LevelTracker.level3Passed)
                        {
                            SceneManager.LoadScene("BossLevel", LoadSceneMode.Single);
                        }
                        break;
                }
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player")) // when the player steps on the button
        {
            button.transform.position += Vector3.down * 2 * Time.deltaTime; // depress the button
            slime.GetComponent<Slime>().StartTalking();
            levelLabel.SetActive(true);
            levelInfo.SetActive(true);
            onButton = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if(other.gameObject.CompareTag("Player")) // when the player steps off the button
        {
            button.transform.position += Vector3.up * 2 * Time.deltaTime; // unpush the button
            levelInfo.SetActive(false);
            levelLabel.SetActive(false);
            slime.GetComponent<Slime>().StopTalking();
            onButton = false;
        }
    }
}
