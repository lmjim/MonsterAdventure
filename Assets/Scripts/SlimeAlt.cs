using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlimeAlt : MonoBehaviour
{
    public Text slimeText;

    public void ShowMessage()
    {
        slimeText.gameObject.SetActive(true);
    }

    public void RemoveMessage()
    {
        slimeText.gameObject.SetActive(false);
    }
}
