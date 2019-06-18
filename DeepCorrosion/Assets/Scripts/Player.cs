using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float maxSpeedXZ = 0.7f;
    public float accelerationXZ = 0.05f;
    public float maxSpeedY = 0.5f;
    public float accelerationY = 0.1f;    
    public float minSpeedLimit = 0.0025f;


    public GameObject grabUI;
    GameObject grabbedBarrelGO;
    
    BoxCollider grabCollider;
    [Header("Useful Variables ( Do not touch them) ")]
    //public float speed = 0.0f;

    public Vector3 speed = Vector3.zero;
    public bool barrelGrabbed = false;


    // Start is called before the first frame update
    void Awake()
    {
        grabCollider = GetComponent<BoxCollider>();
        grabbedBarrelGO = transform.GetChild(0).gameObject;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //-------------------------------------------------------------GRABBING
        if(barrelGrabbed)
        {
            if (Input.GetKey(KeyCode.E))
            {
            }
        }

        //-------------------------------------------------------------INPUT MOVEMENT

        //------------------------------------------- Z AXIS
        if (Input.GetKey(KeyCode.W) && speed.z < maxSpeedXZ) //forward
        {
            speed.z += accelerationXZ * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.S) && speed.z < maxSpeedXZ)//backwards
        {
            speed.z -= accelerationXZ * Time.deltaTime;
        }
        else //deaccelerate if no key pressed
        {
            if (speed.z > 0)
            {
                if (speed.z < minSpeedLimit)
                {
                    speed.z = 0;
                }
                else
                {
                    speed.z -= accelerationXZ * Time.deltaTime;
                }
            }
            else if (speed.z < 0)
            {
                if (speed.z > minSpeedLimit)
                {
                    speed.z = 0;
                }
                else
                {
                    speed.z += accelerationXZ * Time.deltaTime;
                }
            }            
        }

        //------------------------------------------- X AXIS
        if (Input.GetKey(KeyCode.D) && speed.x < maxSpeedXZ) //right
        {
            speed.x += accelerationXZ * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.A) && speed.x < maxSpeedXZ)//left
        {
            speed.x -= accelerationXZ * Time.deltaTime;
        }
        else //deaccelerate if no key pressed
        {
            if (speed.x > 0)
            {
                if (speed.x < minSpeedLimit)
                {
                    speed.x = 0;
                }
                else
                {
                    speed.x -= accelerationXZ * Time.deltaTime;
                }
            }
            else if (speed.x < 0)
            {
                if (speed.x > minSpeedLimit)
                {
                    speed.x = 0;
                }
                else
                {
                    speed.x += accelerationXZ * Time.deltaTime;
                }
            }
        }
        //-------------------------------------------Y AXIS
        if (Input.GetKey(KeyCode.Space) && speed.y < maxSpeedY) //float
        {
            speed.y += accelerationY * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.LeftControl) && speed.y < maxSpeedY)//sink
        {
            speed.y -= accelerationY * Time.deltaTime;
        }
        else //no key pressed
        {
            if (speed.y > 0)
            {
                if (speed.y < minSpeedLimit)
                {
                    speed.y = 0;
                }
                else
                {
                    speed.y -= accelerationY * Time.deltaTime;
                }
            }
            else if (speed.y < 0)
            {
                if (speed.y > minSpeedLimit)
                {
                    speed.y = 0;
                }
                else
                {
                    speed.y += accelerationY * Time.deltaTime;
                }
            }
        }

        
        transform.position = new Vector3(speed.x + transform.position.x, speed.y + transform.position.y, speed.z + transform.position.z);
        //-----------------------------------------------------INPUT MOVEMENT END
    }

    private void OnTriggerEnter(Collider other)
    {
       // Debug.Log("trigger");
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Barrels")// BARREL GRABBED
        {
            grabUI.SetActive(true);
            if (Input.GetKey(KeyCode.E))
            {
                other.gameObject.SetActive(false);
                barrelGrabbed = true;
                grabUI.SetActive(false);
                grabbedBarrelGO.SetActive( true);
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Barrels")
        {
            grabUI.SetActive(false);
        }
    }
}