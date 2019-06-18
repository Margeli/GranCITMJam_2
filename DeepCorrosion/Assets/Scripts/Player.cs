using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float maxSpeed = 0.7f;
    public float accelerationXZ = 0.05f;
    public float accelerationY = 0.1f;
    public float rotate_sensitivity = 0.6f;


    public GameObject grabUI;
    public GameObject dropUI;
    GameObject grabbedBarrelGO;
    public GameObject grabbedBarrelInstantiate;
    
    BoxCollider grabCollider;
    [Header("Useful Variables ( Do not touch them) ")]
    //public float speed = 0.0f;

    public Vector3 speed = Vector3.zero;
    public bool grabbedBarrelBool = false;


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
        if(grabbedBarrelBool)
        {
            if (Input.GetMouseButtonDown(1))//Drop barrel
            {
                //grabbedBarrelInstantiate.SetActive(true);
                GameObject inst = Instantiate(grabbedBarrelInstantiate, transform.parent,true);
                //grabbedBarrelInstantiate.SetActive(false);
                inst.transform.position = transform.position + transform.forward * 1.25f ;  
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

        transform.LookAt(transform.position + trans.forward);
    }

    private void OnTriggerEnter(Collider other)
    {
       // Debug.Log("trigger");
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
                    Destroy(other.gameObject);
                    grabbedBarrelBool = true;
                    grabUI.SetActive(false);
                    grabbedBarrelGO.SetActive(true);
                }
            }
        }
        if(grabbedBarrelBool && other.gameObject.tag == "Submarine")
        {
            dropUI.SetActive(true);
            if (Input.GetKeyDown(KeyCode.E))//drop barrel in the subamrine
            {
                grabbedBarrelGO.SetActive(false);
                grabbedBarrelBool = false;
                dropUI.SetActive(false);
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