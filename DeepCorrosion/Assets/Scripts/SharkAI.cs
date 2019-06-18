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
    private bool can_attack = true;


    void Start()
    {
        target = transform.position;
        state = SharkState.RESTING;
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void FixedUpdate()
    {

        if (state == SharkState.RESTING)
            return;
        else
        {
            Vector3 direction = (target - transform.position).normalized;
            switch (state)
            {
                case SharkState.ROAMING:

                    transform.position += direction * Time.fixedDeltaTime * (avg_speed * 0.5f);

                    break;

                case SharkState.HUNTING:

                    transform.position += direction * Time.fixedDeltaTime * (can_attack ? avg_speed : avg_speed * 0.5f);

                    if (can_attack)
                    {
                        if (Vector3.Distance(target, transform.position) < 2.5f)
                        {
                            // attack player
                            can_attack = false;
                            Invoke("ResetAttack", attack_timer);
                        }
                    }

                    break;

                case SharkState.FLEEING:

                    transform.position += -direction * Time.fixedDeltaTime * avg_speed * 1.25f;

                    break;
            }

            transform.LookAt(Vector3.Lerp(transform.forward, transform.position + direction, 0.1f));
        }
    }

    private void ResetAttack()
    {
        can_attack = true;
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
                    
                    if(Vector3.Distance(transform.position, target) < 1.0f)
                        target = getRoamingTarget();
                    
                    break;
                    
                case SharkState.HUNTING:

                    target = player.transform.position;

                    RaycastHit[] hits = Physics.RaycastAll(transform.position, target - transform.position, Mathf.Min(Vector3.Distance(target, transform.position), hunting_distance));

                    bool found = false;
                    if(hits.Length == 1)
                    {
                        if (hits[0].collider.gameObject.tag == "Player")
                            found = true;
                    }

                    if (!found)
                        state = SharkState.ROAMING;

                    break;

                case SharkState.FLEEING:

                    target = player.transform.position;

                    if (Vector3.Distance(player.transform.position, transform.position) > hunting_distance)
                        state = SharkState.ROAMING;

                    break;

                default:
                    break;
            }
        }
    }



    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player" && state == SharkState.RESTING)
            state = SharkState.ROAMING;
        
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player" && state == SharkState.ROAMING)
            state = SharkState.RESTING;
        
    }


    private Vector3 getRoamingTarget()
    {
        Vector3 target = transform.position;
        int counter = 0;

        while (target != transform.position && counter <= 5)
        {
            target = Random.insideUnitSphere * Random.Range(roam_min_dist, roam_max_dist) + transform.position;
            if(Physics.OverlapBox(target, Vector3.one).Length > 0)
                target = transform.position;
        }

        return target;
    }

}
