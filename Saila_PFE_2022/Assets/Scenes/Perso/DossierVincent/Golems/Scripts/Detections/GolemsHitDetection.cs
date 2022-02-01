using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolemsHitDetection : MonoBehaviour
{
    [HideInInspector]
    public bool playerHit;
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && !playerHit)
        {
            playerHit = true;
        }
    }
}
