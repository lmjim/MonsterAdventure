/*
references:
https://docs.unity3d.com/2019.1/Documentation/ScriptReference/UI.Toggle-onValueChanged.html
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSelection : MonoBehaviour
{
    public static bool fiery;
    public static bool cyclops;

    private Toggle fieryToggle;
    private Toggle cyclopsToggle;

    void Start()
    {
        fieryToggle = transform.GetChild(0).gameObject.GetComponent<Toggle>();
        cyclopsToggle = transform.GetChild(1).gameObject.GetComponent<Toggle>();

        fieryToggle.onValueChanged.AddListener(delegate {
            ToggleValueChanged(fieryToggle);
        });
        cyclopsToggle.onValueChanged.AddListener(delegate {
            ToggleValueChanged(cyclopsToggle);
        });

        fiery = fieryToggle.isOn;
        cyclops = cyclopsToggle.isOn;
    }

    void ToggleValueChanged(Toggle change)
    {
        fiery = fieryToggle.isOn;
        cyclops = cyclopsToggle.isOn;
    }
}
