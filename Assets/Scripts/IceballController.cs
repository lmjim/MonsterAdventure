using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceballController : MonoBehaviour
{
    void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Footman_Blue"))
        {
               Destroy(gameObject);
        }
    }
}
