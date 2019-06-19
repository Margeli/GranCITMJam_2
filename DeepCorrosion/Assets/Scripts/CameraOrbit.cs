using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraOrbit : MonoBehaviour
{
   
    void Update()
    {
        transform.RotateAround(Vector3.zero, Vector3.up, 0.02f);
        transform.LookAt(Vector3.zero);
    }
}
