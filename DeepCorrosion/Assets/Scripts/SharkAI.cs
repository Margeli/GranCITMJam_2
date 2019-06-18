using UnityEngine;

enum SharkState { RESTING, ROAMING, HUNTING, FLEEING};

public class SharkAI : MonoBehaviour
{

    public float avg_speed = 3.0f;
    public float roam_min_dist = 15.0f;
    public float roam_max_dist = 35.0f;
    public float hunting_distance = 20.0f;
    public float attack_timer = 3.0f;

    private GameObject player;
    private SharkState state;
    private Vector3 target;
    private Vector3 origin;
    private bool can_attack = true;


    void Start()
    {
        target = origin = transform.position;
        state = SharkState.RESTING;
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void FixedUpdate()
    {

        if (state == SharkState.RESTING)
            return;
        else
        {
            Vector3 direction = (state == SharkState.ROAMING ? target - transform.position : player.transform.position - transform.position).normalized;

            switch (state)
            {
                case SharkState.ROAMING:

                    transform.position += direction * Time.fixedDeltaTime * (avg_speed * 0.5f);

                    break;

                case SharkState.HUNTING:

                    transform.position += direction * Time.fixedDeltaTime * (can_attack ? avg_speed : avg_speed * 0.5f);

                    break;

                case SharkState.FLEEING:

                    transform.position += -direction * Time.fixedDeltaTime * avg_speed * 1.25f;

                    break;
            }

            transform.LookAt(Vector3.Lerp(transform.position + transform.forward, transform.position + direction, 0.1f));
        }
    }
    

    // Update is called once per frame
    private void Update()
    {
        if (state == SharkState.RESTING)
            return;
        else
        {
            switch(state)
            {
                case SharkState.ROAMING:

                    if (Vector3.Distance(target, transform.position) < 1.0f)
                        target = getRoamingTarget();

                    if (Vector3.Distance(player.transform.position, transform.position) < hunting_distance)
                        state = SharkState.HUNTING;
                    
                    break;
                    
                case SharkState.HUNTING:

                    target = player.transform.position;

                    if(Vector3.Distance(target, transform.position) > hunting_distance)
                    {
                        state = SharkState.ROAMING;
                        target = transform.position;
                        return;
                    }

                    RaycastHit[] hits = Physics.RaycastAll(transform.position, target - transform.position, Mathf.Min(Vector3.Distance(target, transform.position), hunting_distance));

                    bool found = false;
                    if(hits.Length == 1)
                    {
                        if (hits[0].collider.gameObject.tag == "Player")
                            found = true;
                    }

                    if (!found)
                    {
                        state = SharkState.ROAMING;
                        target = transform.position;
                    }

                    break;

                case SharkState.FLEEING:

                    if (Vector3.Distance(player.transform.position, transform.position) > hunting_distance)
                    {
                        state = SharkState.ROAMING;
                        target = transform.position;
                    }

                    break;

                default:
                    break;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player" && state == SharkState.HUNTING)
        {
            if (can_attack)
            {
               // attack player
               can_attack = false;
               Invoke("ResetAttack", attack_timer);
            }
        }
    }


    private void ResetAttack()
    {
        can_attack = true;
    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player" && state == SharkState.RESTING)
            state = SharkState.ROAMING;
        
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player" && state == SharkState.ROAMING)
        {
            state = SharkState.RESTING;
            transform.position = origin;
        }
        
    }


    private Vector3 getRoamingTarget()
    {
        Vector3 target = transform.position;
        int counter = 0;
        
        while (target == transform.position && counter <= 5)
        {
            target = origin + new Vector3(Random.Range(roam_min_dist, roam_max_dist), Random.Range(roam_min_dist * 0.2f, roam_max_dist * 0.2f), Random.Range(roam_min_dist, roam_max_dist));

            Collider[] collisions = Physics.OverlapBox(target, Vector3.one);

            for(int i = 0; i < collisions.Length; i++)
                if(!collisions[i].isTrigger)
                {
                    target = transform.position;
                    break;
                }
            

            counter++;
        }

        return target;
    }

}
