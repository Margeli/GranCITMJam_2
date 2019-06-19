using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mine : MonoBehaviour
{

    public GameObject explosion;
    public bool triggered = false;


    public float time_to_explode_after_detection = 3;
    public float light_frequency = 0.5f;
    public float MaxDmg = 100.0f;

    private float elapsed_time = 0.0f;
    private GameObject light_go;
    public float explosionRadius = 8.0f;
    private GameObject player;

    
    // Start is called before the first frame update
    void Start()
    {
        light_go = GameObject.Find("Mine Lights");
        light_go.SetActive(false);
       
        
    }

    // Update is called once per frame
    void Update()
    {
        
        if (triggered)
        {
            elapsed_time += Time.deltaTime;
            if (elapsed_time > time_to_explode_after_detection)
                Explode();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!triggered)
            {                
                StartCoroutine(Countdown());
            }
            triggered = true;
            player = other.gameObject;
        }
    }

    IEnumerator Countdown()
    {

        while (true)
        {
            if (light_go.activeSelf)
                light_go.SetActive(false);
            else
                light_go.SetActive(true);
            yield return new WaitForSeconds(light_frequency);
        }
    }

    private void Explode()
    {
        GameObject firework = Instantiate(explosion, transform.position, Quaternion.identity);
        firework.GetComponent<ParticleSystem>().Play();

        float radius = GetComponent<SphereCollider>().radius;
        Vector3 dist = transform.position - player.transform.position;
        if (dist.magnitude < explosionRadius)
        {
            float percent = (dist.magnitude) * 100 / (explosionRadius);
            player.GetComponent<Player>().health -= percent * MaxDmg;
            
        }

        MeshRenderer mesh_object = GetComponentInChildren<MeshRenderer>();
        mesh_object.gameObject.SetActive(false);
        Destroy(firework, 5);
        Destroy(this.gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        
    
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position, explosionRadius);
    }
}
