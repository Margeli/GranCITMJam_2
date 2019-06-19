using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireColumn : MonoBehaviour
{

    private Player player;
    private bool damaging = false;

    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
            damaging = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
            damaging = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(damaging && !player.has_heat_shield)
            player.health -= 0.01f;
    }
}
