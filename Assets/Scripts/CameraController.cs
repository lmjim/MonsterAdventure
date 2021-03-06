﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject players;
    private GameObject player;

    private Vector3 offset;

    // Start is called before the first frame update
    void Start()
    {
        player = PlayerSwitch.DefinePlayer(players);
        offset = new Vector3 (0, 5, -5); // defined vector so the camera is always the same between levels
        transform.position = player.transform.position + offset;
    }

    // LateUpdate is called once per frame, but can be a bit late
    void LateUpdate()
    {
        transform.position = player.transform.position + offset;
    }
}