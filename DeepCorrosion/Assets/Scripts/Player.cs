﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public float maxSpeed = 0.7f;
    public float accelerationXZ = 0.05f;
    public float accelerationY = 0.01f;
    public float rotate_sensitivity = 0.6f;
    public float health = 100.0f;
    public int initialDepth = 1120;

    GameObject canvas;
    GameObject grabUI;
    GameObject dropUI;
    Text depthUI;
    Text barrelsLeftUI;
    Text healthUI;
    GameObject grabbedBarrelGO;
    public GameObject grabbedBarrelInstantiate;
    public int totalBarrels = 2;    
    BoxCollider grabCollider;

    ElectricStick electricStickScript;


    [Header("Useful Variables ( Do not touch them) ")]
    //public float speed = 0.0f;

    public Vector3 speed = Vector3.zero;
    public bool grabbedBarrelBool = false;
    public bool attacking = false;
    private Rigidbody rb;



    // Start is called before the first frame update
    void Awake()
    {
        grabCollider = GetComponent<BoxCollider>();
        grabbedBarrelGO = transform.GetChild(0).gameObject;

        canvas = GameObject.Find("Canvas");

        electricStickScript = transform.GetChild(1).gameObject.GetComponent<ElectricStick>();
        grabUI = canvas.transform.Find("GrabBarrelText").gameObject;
        dropUI = canvas.transform.Find("DropBarrelText").gameObject;
        barrelsLeftUI = canvas.transform.Find("BarrelsLeftText").gameObject.transform.GetChild(0).GetComponent<Text>();
        barrelsLeftUI.text = totalBarrels.ToString();
        depthUI = canvas.transform.Find("WaterDepthText").gameObject.transform.GetChild(0).GetComponent<Text>();
        depthUI.text = initialDepth.ToString();
        healthUI = canvas.transform.Find("HealthText").gameObject.transform.GetChild(0).GetComponent<Text>();
        healthUI.text = health.ToString();
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rb.velocity = Vector3.zero;
        //-------------------------------------------------------------GRABBING
        if (!grabbedBarrelBool)
        {
            if (Input.GetMouseButtonDown(0))
            {
                attacking = true;
                electricStickScript.Attack();
            }       
        }
        else
        {
            if (Input.GetMouseButtonDown(1))//Drop barrel
            {
                GameObject inst = Instantiate(grabbedBarrelInstantiate, transform.parent, true);
                inst.transform.position = transform.position + transform.forward * 1.25f;
                grabbedBarrelGO.SetActive(false);
                grabbedBarrelBool = false;
            }
        }

        //-------------------------------------------------------------INPUT MOVEMENT

        Vector3 movement = Vector3.zero;
        movement.x = Input.GetAxis("Horizontal") * accelerationXZ * Time.deltaTime;
        movement.z = Input.GetAxis("Vertical") * accelerationXZ * Time.deltaTime;

        if (Input.GetKey(KeyCode.Space))
            movement.y = accelerationY * Time.deltaTime;
        else if (Input.GetKey(KeyCode.LeftControl))
            movement.y = -accelerationY * Time.deltaTime;

        movement = transform.rotation * movement;

        if (grabbedBarrelBool)
            movement *= 0.3f;

        speed += movement;

        if (speed.magnitude > maxSpeed)
            speed = speed.normalized * maxSpeed;

        if (movement == Vector3.zero)
            speed *= 0.95f;

        transform.position += speed;

        //-----------------------------------------------------INPUT MOVEMENT END

        // Camera rotation
        Transform trans = transform;

        Vector2 deltaMouse = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

        trans.Rotate(Vector3.up, deltaMouse.x * rotate_sensitivity);
        trans.Rotate(Vector3.right, -deltaMouse.y * rotate_sensitivity);
        

        if (trans.rotation.eulerAngles.x < 180.0f && trans.rotation.eulerAngles.x > 20.0f)
            trans.rotation = Quaternion.Euler(20.0f, trans.rotation.eulerAngles.y, 0);
        else if (trans.rotation.eulerAngles.x > 180.0f && trans.rotation.eulerAngles.x < 340.0f)
            trans.rotation = Quaternion.Euler(340.0f, trans.rotation.eulerAngles.y, 0);

        transform.LookAt(transform.position + trans.forward);

        //------UI
        depthUI.text = (initialDepth - (int)transform.position.y).ToString() ;
        healthUI.text = health.ToString();
    }

    public void EndAttack()
    {
        attacking = false;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Barrels")// BARREL GRABBED
        {
            if (!grabbedBarrelBool)
            {
                grabUI.SetActive(true);
                if (Input.GetKeyDown(KeyCode.E))//pick barrel
                {
                    electricStickScript.gameObject.SetActive(false);
                    Destroy(other.gameObject);
                    grabbedBarrelBool = true;
                    grabUI.SetActive(false);
                    grabbedBarrelGO.SetActive(true);
                }
            }
        }
        if(grabbedBarrelBool && other.gameObject.tag == "DeliverSpot")
        {
            dropUI.SetActive(true);
            if (Input.GetKeyDown(KeyCode.E))//drop barrel in the subamrine
            {
                electricStickScript.gameObject.SetActive(true);
                grabbedBarrelGO.SetActive(false);
                grabbedBarrelBool = false;
                dropUI.SetActive(false);
                totalBarrels--;
                barrelsLeftUI.text = totalBarrels.ToString();
                other.gameObject.GetComponent<BarrelDelivery>().SetBarrel();
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