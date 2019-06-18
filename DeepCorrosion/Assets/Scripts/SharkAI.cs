using UnityEngine;

enum SharkState { RESTING, ROAMING, HUNTING, FLEEING};

public class SharkAI : MonoBehaviour
{

    public float avg_speed = 3.0f;
    public float roaming_distance = 30.0f;
    public float hunting_distance = 20.0f;
    public float resting_distance = 60.0f;
    public float detection_distance = 40.0f;
    public float attack_timer = 4.0f;
    public float collision_detection_dist = 2.0f;

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
        transform.GetChild(0).gameObject.SetActive(false);
    }

    void FixedUpdate()
    {

        if (state == SharkState.RESTING)
            return;
        else
        {
            Vector3 direction =  (target - transform.position).normalized;

            if (state == SharkState.FLEEING)
                direction = -direction;

            direction = CollisionDetection(direction);

            switch (state)
            {
                case SharkState.ROAMING:

                    transform.position += direction * Time.fixedDeltaTime * (avg_speed * 0.5f);

                    break;

                case SharkState.HUNTING:

                    transform.position += direction * Time.fixedDeltaTime * (can_attack ? avg_speed : avg_speed * 0.1f);

                    break;

                case SharkState.FLEEING:

                    transform.position += direction * Time.fixedDeltaTime * avg_speed * 1.25f;

                    break;
            }

            transform.LookAt(Vector3.Lerp(transform.position + transform.forward, transform.position + direction, 0.05f));
        }
    }
    

    // Update is called once per frame
    private void Update()
    {
        if (state == SharkState.RESTING && Vector3.Distance(player.transform.position, transform.position) < detection_distance)
        {
            state = SharkState.ROAMING;
            transform.GetChild(0).gameObject.SetActive(true);
        }
        else
        {
            switch (state)
            {
                case SharkState.ROAMING:

                    if (Vector3.Distance(target, transform.position) < 1.0f)
                        target = getRoamingTarget();

                    else if (Vector3.Distance(player.transform.position, transform.position) < hunting_distance)
                        state = SharkState.HUNTING;


                    else if (Vector3.Distance(player.transform.position, transform.position) > resting_distance)
                    {
                        state = SharkState.RESTING;
                        transform.position = origin;
                        transform.GetChild(0).gameObject.SetActive(false);
                    }

                    break;

                case SharkState.HUNTING:

                    target = player.transform.position;

                    if (Vector3.Distance(target, transform.position) > hunting_distance || Vector3.Distance(transform.position, origin) > detection_distance)
                    {
                        state = SharkState.ROAMING;
                        target = transform.position;
                        return;
                    }

                    RaycastHit[] hits = Physics.RaycastAll(transform.position, target - transform.position, Mathf.Min(Vector3.Distance(target, transform.position), hunting_distance));

                    bool found = false;
                    for (int i = 0; i < hits.Length; i++)
                    {
                        if (hits[i].collider.gameObject != gameObject && hits[i].collider.gameObject.tag != "Player")
                        {
                            found = false;
                            break;
                        }

                        else if (hits[i].collider.gameObject.tag == "Player")
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
    


    private void ResetAttack()
    {
        can_attack = true;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && state == SharkState.HUNTING)
        {
            if (can_attack)
            {
                RaycastHit hit;
                if (Physics.Raycast(transform.position, transform.forward, out hit))
                {
                    Player player_script = player.GetComponent<Player>();
                    player_script.health -= 20.0f;
                    player_script.speed = hit.normal.normalized * player_script.maxSpeed;
                    can_attack = false;
                    Invoke("ResetAttack", attack_timer);
                }
            }
        }
    }


    private Vector3 getRoamingTarget()
    {
        Vector3 target = transform.position;
        int counter = 0;
        
        while (target == transform.position && counter <= 5)
        {
            target = origin + new Vector3(Random.Range(-roaming_distance, roaming_distance), Random.Range(-roaming_distance * 0.2f, roaming_distance * 0.2f), Random.Range(-roaming_distance, roaming_distance));

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

    private Vector3 CollisionDetection(Vector3 direction)
    {
        RaycastHit[] hits = Physics.RaycastAll(transform.position, direction, collision_detection_dist);

        for (int i = 0; i < hits.Length; i++)
        {
            if (hits[i].collider.gameObject != gameObject && hits[i].collider.gameObject.tag != "Player")
                direction = (direction * hits[0].distance + hits[0].normal.normalized).normalized;
        }

        return direction;
    }

    public void Flee() {
        state = SharkState.FLEEING;
    }

}
