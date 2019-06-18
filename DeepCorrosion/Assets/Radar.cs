using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Radar : MonoBehaviour
{
    GameObject player;
    private float radar_frequency = 3.0f;
    private float timer = 0.0f;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= radar_frequency)
        {
            RadarSwipe();
        }
    }

    void RadarSwipe()
    {
        Transform playertrans = player.transform;
        //Collider[] hitColliders = Physics.OverlapSphere(center, radius);
    }
}
