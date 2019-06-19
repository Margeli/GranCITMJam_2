using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishSchool : MonoBehaviour
{
    public float speed = 1.0f;
    private Vector3 direction;

    void Start()
    {
        direction = new Vector3(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f));
        direction = direction.normalized;
    }

    private void OnTriggerEnter(Collider other)
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit))
            direction = hit.normal.normalized;
    }

    void Update()
    {
        transform.position += direction * speed * Time.deltaTime;
        transform.LookAt(transform.position + Vector3.Lerp(transform.forward, direction, 0.05f));
    }
}
