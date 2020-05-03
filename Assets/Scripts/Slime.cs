using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slime : MonoBehaviour
{

    public string message;
    public Text slimeText;

    private Animator slimeAnimator;

    void Start()
    {
        slimeAnimator = GetComponent<Animator>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            slimeText.text = message;
            slimeAnimator.SetBool("Talking", true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            slimeText.text = "";
            slimeAnimator.SetBool("Talking", false);
        }
    }

    public void FinishLevel()
    {
        slimeAnimator.SetTrigger("Dance");
    }

    public void PlayerDied()
    {
        slimeAnimator.SetTrigger("SenseSomething");
    }
}
