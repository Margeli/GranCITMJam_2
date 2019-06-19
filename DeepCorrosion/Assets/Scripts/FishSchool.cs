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
        direction = new Vector3(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f));
        direction = direction.normalized;
    }

    void Update()
    {
        transform.position += direction * speed * Time.deltaTime;
    }
}
