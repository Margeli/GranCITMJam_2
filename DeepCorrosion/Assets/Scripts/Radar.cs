using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Radar : MonoBehaviour
{
    GameObject player;
    public float radar_radius = 5.0f;
    public float radar_frequency = 3.0f;
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
            timer = 0.0f;
        }
    }

    void RadarSwipe()
    {
        Transform playertrans = player.transform;
        Collider[] hitColliders = Physics.OverlapSphere(playertrans.position, radar_radius, -1, QueryTriggerInteraction.Ignore);
        foreach (Collider col in hitColliders)
        {
            if (col.tag == "Enemy")
            {
                Debug.Log("enemy in range");
            }
        }
    }
}
