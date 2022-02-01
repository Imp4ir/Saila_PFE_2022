using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolemOnPatrolPointDetection : MonoBehaviour
{
    public bool golemOnPoint;
    public bool golemOnItsWayToPoint;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            golemOnPoint = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            golemOnPoint = false;
        }
    }
}
