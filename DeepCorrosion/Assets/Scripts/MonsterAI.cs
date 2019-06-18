using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum MonsterState { PATROLLING, ROAMING, HUNTING };

public class MonsterAI : MonoBehaviour
{

    public float avg_speed = 5.0f;
    public float roaming_distance = 30.0f;
    public float hunting_distance = 20.0f;
    public float ignoring_distance = 60.0f;
    public float detection_distance = 40.0f;
    public float attack_timer = 4.0f;
    public float collision_detection_dist = 4.0f;
    public float patrol_time = 60.0f;
    public float push_speed = 1.0f;

    private GameObject player;
    private MonsterState state;
    private Vector3 target;
    private Vector3 origin;
    private bool can_attack = true;
    private int patrol_index = 0;
    private float timer = 0.0f;

    private Vector3[] patrol_points;


    void Start()
    {
        target = origin = transform.position;
        state = MonsterState.PATROLLING;
        player = GameObject.FindGameObjectWithTag("Player");
        GetComponent<SphereCollider>().radius = detection_distance;

        GameObject[] ptrl_objects = GameObject.FindGameObjectsWithTag("PatrolPoint");
        patrol_points = new Vector3[ptrl_objects.Length];

        for(int i = 0; i < ptrl_objects.Length; i++)
            patrol_points[ptrl_objects[i].GetComponent<PatrolPoint>().index] = ptrl_objects[i].transform.position;

        target = patrol_points[0];
        
    }

    void FixedUpdate()
    {
        Vector3 direction = (target - transform.position).normalized;

        direction = CollisionDetection(direction);

        switch (state)
        {
            case MonsterState.ROAMING:

                transform.position += direction * Time.fixedDeltaTime * (avg_speed * 0.5f);

                break;

            case MonsterState.HUNTING:

                transform.position += direction * Time.fixedDeltaTime * (can_attack ? avg_speed : avg_speed * 0.5f);

                break;

            case MonsterState.PATROLLING:

                transform.position += direction * Time.fixedDeltaTime * avg_speed * 1.25f;

                break;
        }

        transform.LookAt(Vector3.Lerp(transform.position + transform.forward, transform.position + direction, 0.05f));

    }


    // Update is called once per frame
    private void Update()
    {

        switch (state)
        {
            case MonsterState.PATROLLING:

                if (Vector3.Distance(target, transform.position) < 0.5f)
                {
                    state = MonsterState.ROAMING;
                    origin = patrol_points[patrol_index];
                    target = transform.position;
                    timer = 0.0f;
                }
                else if (Vector3.Distance(player.transform.position, transform.position) < hunting_distance)
                {
                    state = MonsterState.HUNTING;
                    origin = transform.position;
                }

                break;

            case MonsterState.ROAMING:

                if (Vector3.Distance(target, transform.position) < 1.0f)
                    target = getRoamingTarget();

                else if (Vector3.Distance(player.transform.position, transform.position) < hunting_distance)
                    state = MonsterState.HUNTING;

                timer += Time.deltaTime;

                if (timer > patrol_time)
                {
                    target = patrol_points[patrol_index];

                    if (Vector3.Distance(target, transform.position) < 0.5f)
                    {
                        patrol_index++;
                        if (patrol_index == patrol_points.Length)
                            patrol_index = 0;

                        target = patrol_points[patrol_index];
                        state = MonsterState.PATROLLING;
                    }
                }


                break;

            case MonsterState.HUNTING:

                target = player.transform.position;

                if (Vector3.Distance(target, transform.position) > hunting_distance || Vector3.Distance(transform.position, origin) > detection_distance)
                {
                    state = MonsterState.ROAMING;
                    target = origin;
                    timer = 0.0f;
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
                    state = MonsterState.ROAMING;
                    target = transform.position;
                    timer = 0.0f;
                }

                break;

            default:
                break;
        }

    }


   

    private void ResetAttack()
    {
        can_attack = true;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && state == MonsterState.HUNTING)
        {
            if (can_attack)
            {
                RaycastHit hit;
                if (Physics.Raycast(transform.position, transform.forward, out hit))
                {
                    Player player_script = player.GetComponent<Player>();
                    player_script.health -= 45.0f;
                    player_script.speed = -hit.normal.normalized * push_speed;
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

            for (int i = 0; i < collisions.Length; i++)
                if (!collisions[i].isTrigger)
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

}
