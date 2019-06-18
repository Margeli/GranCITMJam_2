﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickCollider : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

  
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Enemy")
        {
            SharkAI shark = other.gameObject.GetComponent<SharkAI>();
            if (shark)
            {
                shark.Flee();
            }
            
        }
    }
}
