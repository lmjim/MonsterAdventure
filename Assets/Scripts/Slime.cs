using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slime : MonoBehaviour
{

    public string message;
    public Text slimeText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowMessage()
    {
        slimeText.text = message;
    }

    public void RemoveMessage()
    {
        slimeText.text = "";
    }
}
