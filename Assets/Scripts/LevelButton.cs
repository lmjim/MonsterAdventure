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

    private AudioSource audioSource;
    public AudioClip button_sound;
    public AudioClip selection_sound;

    public static bool onLevel1 = false;
    void Start()
    {
        button = transform.GetChild(0).gameObject;
        audioSource = GetComponent<AudioSource>();

    }

    void Update()
    {
        if(onButton)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                StartCoroutine(SwitchAndWait());

                // Moved into SwitchAndWait coroutine
                /*
                switch (level)
                {
                    case 0: // tutorial level

                        SceneManager.LoadScene("TutorialLevel", LoadSceneMode.Single);
                        break;
                    case 1: // level 1
                        SceneManager.LoadScene("Level1", LoadSceneMode.Single);
                        onLevel1 = true;
                        break;
                    case 2: // level 2
                        if (LevelTracker.level1Passed)
                        {
                            SceneManager.LoadScene("Level2", LoadSceneMode.Single);
                        }
                        break;
                    case 3: // level 3
                        if (LevelTracker.level2Passed)
                        {
                            SceneManager.LoadScene("Level3", LoadSceneMode.Single);
                        }
                        break;
                    case 4: // boss level
                        if (LevelTracker.level3Passed)
                        {
                            SceneManager.LoadScene("BossLevel", LoadSceneMode.Single);
                        }
                        break;
                }
                */
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player")) // when the player steps on the button
        {
            button.transform.position += Vector3.down * 2 * Time.deltaTime; // depress the button
            audioSource.PlayOneShot(button_sound);
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

    IEnumerator SwitchAndWait() // Coroutine to yield before switching scene so sound fx can finish playing out
    {
        // Playing selection sound fx if applicable
        switch (level)
        {
            case 0:
                audioSource.PlayOneShot(selection_sound);
                break;
            case 1:
                audioSource.PlayOneShot(selection_sound);
                break;
            case 2:
                if (LevelTracker.level1Passed)
                {
                    audioSource.PlayOneShot(selection_sound); ;
                }
                break;
            case 3:
                if (LevelTracker.level2Passed)
                {
                    audioSource.PlayOneShot(selection_sound);
                }
                break;
        }
        
        // Waiting for one second before switching scenes
        yield return new WaitForSeconds(1.0f); 
        switch (level)
        {
            case 0: // tutorial level

                SceneManager.LoadScene("TutorialLevel", LoadSceneMode.Single);
                break;
            case 1: // level 1
                SceneManager.LoadScene("Level1", LoadSceneMode.Single);
                onLevel1 = true;
                break;
            case 2: // level 2
                if (LevelTracker.level1Passed)
                {
                    SceneManager.LoadScene("Level2", LoadSceneMode.Single);
                }
                break;
            case 3: // level 3
                if (LevelTracker.level2Passed)
                {
                    SceneManager.LoadScene("Level3", LoadSceneMode.Single);
                }
                break;
            case 4: // boss level
                if (LevelTracker.level3Passed)
                {
                    SceneManager.LoadScene("BossLevel", LoadSceneMode.Single);
                }
                break;
        }
    }
}
