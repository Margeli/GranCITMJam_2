using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public float maxSpeed = 0.7f;
    public float accelerationXZ = 0.05f;
    public float accelerationY = 0.1f;
    public float rotate_sensitivity = 0.6f;
    public float health = 100.0f;

    public GameObject canvas;
    GameObject grabUI;
    GameObject dropUI;
    Text barrelsLeftUI;
    GameObject grabbedBarrelGO;
    public GameObject grabbedBarrelInstantiate;
    public int totalBarrels = 2;    
    BoxCollider grabCollider;

    ElectricStick electricStickScript;
    bool attacking = false;


    [Header("Useful Variables ( Do not touch them) ")]
    //public float speed = 0.0f;

    public Vector3 speed = Vector3.zero;
    public bool grabbedBarrelBool = false;



    // Start is called before the first frame update
    void Awake()
    {
        grabCollider = GetComponent<BoxCollider>();
        grabbedBarrelGO = transform.GetChild(0).gameObject;
        
        electricStickScript = transform.GetChild(1).gameObject.GetComponent<ElectricStick>();        
        grabUI = canvas.transform.Find("GrabBarrelText").gameObject;
        dropUI = canvas.transform.Find("DropBarrelText").gameObject;
        barrelsLeftUI = canvas.transform.Find("BarrelsLeftText").gameObject.GetComponent<Text>();
        barrelsLeftUI.text = totalBarrels.ToString();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
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
        if(grabbedBarrelBool && other.gameObject.tag == "Submarine")
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