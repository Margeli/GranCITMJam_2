using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public float maxSpeed = 0.7f;
    public float accelerationXZ = 0.3f;
    public float rotate_sensitivity = 0.6f;
    public float health = 100.0f;
    public int initialDepth = 1120;

    public bool hidden = false;
    public bool has_heat_shield = false;

    GameObject canvas;
    GameObject grabUI;
    GameObject dropUI;
    GameObject releaseUI;
    Text depthUI;
    Text barrelsLeftUI;
    Text healthUI;
    GameObject red;
    GameObject grabbedBarrelGO;
    public GameObject grabbedBarrelInstantiate;
    public int totalBarrels = 2;    
    BoxCollider grabCollider;

    ElectricStick electricStickScript;

    public AudioClip grabBarrel;
    public AudioClip deliverBarrel;

    [Header("Useful Variables ( Do not touch them) ")]
    //public float speed = 0.0f;

    public Vector3 speed = Vector3.zero;
    public bool grabbedBarrelBool = false;
    public bool attacking = false;
    private Rigidbody rb;
    private bool regenerate = false;
    public float regeneratePerSec = 10.0f;

    private bool engineON = false;
    private AudioSource engineAudio;

    // Start is called before the first frame update
    void Awake()
    {
        grabCollider = GetComponent<BoxCollider>();
        grabbedBarrelGO = transform.GetChild(0).gameObject;

        canvas = GameObject.Find("Canvas");

        electricStickScript = transform.GetChild(1).gameObject.GetComponent<ElectricStick>();
        grabUI = canvas.transform.Find("GrabBarrelText").gameObject;
        dropUI = canvas.transform.Find("DropBarrelText").gameObject;
        releaseUI = canvas.transform.Find("ReleaseBarrelText").gameObject;
        barrelsLeftUI = canvas.transform.Find("BarrelsLeftText").gameObject.transform.GetChild(0).GetComponent<Text>();
        barrelsLeftUI.text = totalBarrels.ToString();
        depthUI = canvas.transform.Find("WaterDepthText").gameObject.transform.GetChild(0).GetComponent<Text>();
        depthUI.text = initialDepth.ToString();
        healthUI = canvas.transform.Find("HealthText").gameObject.transform.GetChild(0).GetComponent<Text>();
        healthUI.text = health.ToString();
        red = canvas.transform.Find("red").gameObject;
        rb = GetComponent<Rigidbody>();

        engineAudio = transform.Find("Micro_Sub").gameObject.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (regenerate)
        {
            Debug.Log("Regenerate");
            if (health < 100)
            {
                red.SetActive(true);
                health += regeneratePerSec * Time.fixedDeltaTime;
            }
            if (health > 100)
            {
                health = 100;
            }
        }

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
                inst.SetActive(true);
                inst.GetComponent<Rigidbody>().useGravity = true;
                inst.transform.position = transform.position + transform.forward * 1.25f;
                grabbedBarrelGO.SetActive(false);
                grabbedBarrelBool = false;
                releaseUI.SetActive(false);
            }
        }

        //-------------------------------------------------------------INPUT MOVEMENT

        Vector3 movement = Vector3.zero;
        movement.x = Input.GetAxis("Horizontal") * accelerationXZ * Time.deltaTime;
        movement.z = Input.GetAxis("Vertical") * accelerationXZ * Time.deltaTime;

        if (engineON && Mathf.Equals(movement, Vector3.zero))
        {
            StartCoroutine(FadeOut(engineAudio, 0.5f));
            engineON = false;
        }
        else if (!engineON && !Mathf.Equals(movement, Vector3.zero))
        {
            StartCoroutine(FadeIn(engineAudio, 0.5f));
            engineON = true;
        }

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

        transform.LookAt(transform.position + trans.forward);

        //------UI
        depthUI.text = (initialDepth - (int)transform.position.y).ToString() ;
        healthUI.text =((int) health).ToString();
    }

    public void EndAttack()
    {
        attacking = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Linterna")
            GameObject.Find("ElectricStick").SetActive(true);
        else if (other.gameObject.name == "HeatShield")
            has_heat_shield = true;
        else if (other.gameObject.tag == "DeliverSpot")        
            regenerate = true;

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
                    releaseUI.SetActive(true);
                    grabUI.SetActive(false);
                    grabbedBarrelGO.SetActive(true);
                    GetComponent<AudioSource>().PlayOneShot(grabBarrel);
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
                releaseUI.SetActive(false);
                dropUI.SetActive(false);
                totalBarrels--;
                barrelsLeftUI.text = totalBarrels.ToString();
                other.gameObject.GetComponent<BarrelDelivery>().SetBarrel();
                GetComponent<AudioSource>().PlayOneShot(deliverBarrel);
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Barrels")
        {
            grabUI.SetActive(false);
            
        }
        if (other.gameObject.tag == "DeliverSpot")
        {
            regenerate = false;
            red.SetActive(false);
            dropUI.SetActive(false);
        }
    }

    public static IEnumerator FadeOut(AudioSource audioSource, float FadeTime)
    {
        float startVolume = audioSource.volume;
        while (audioSource.volume > 0)
        {
            audioSource.volume -= startVolume * Time.deltaTime / FadeTime;
            yield return null;
        }
        audioSource.Stop();
    }

    public static IEnumerator FadeIn(AudioSource audioSource, float FadeTime)
    {
        audioSource.Play();
        audioSource.volume = 0f;
        while (audioSource.volume < 1)
        {
            audioSource.volume += Time.deltaTime / FadeTime;
            yield return null;
        }
        
    }
   
}