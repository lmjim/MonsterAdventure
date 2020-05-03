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

    public void ShowMessage()
    {
        slimeText.text = message;
    }

    public void RemoveMessage()
    {
        slimeText.text = "";
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
