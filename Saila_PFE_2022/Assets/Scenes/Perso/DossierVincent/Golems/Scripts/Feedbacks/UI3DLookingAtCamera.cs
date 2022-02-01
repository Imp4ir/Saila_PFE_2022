using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI3DLookingAtCamera : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        if (Camera.main is {})
            transform.LookAt(Camera.main.transform);
    }
}
