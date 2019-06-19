using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Radar : MonoBehaviour
{
    public GameObject dot;
    GameObject player;
    public float radar_radius = 100.0f;
    public float radar_frequency = 3.0f;
    private float timer = 0.0f;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= radar_frequency)
        {
            RadarSwipe();
            timer = 0.0f;
        }
    }

    bool RadarSwipe()
    {
        bool detected = false;

        Transform playertrans = player.transform;
        Collider[] hitColliders = Physics.OverlapSphere(playertrans.position, radar_radius, -1, QueryTriggerInteraction.Ignore);
        foreach (Collider col in hitColliders)
        {
            if (col.tag == "Enemy")
            {
                Debug.Log("enemy in range");

                Vector3 posPlayer = player.transform.position;
                Vector3 posEnemy = col.transform.position;

                float diffX = posEnemy.x - posPlayer.x;
                float diffY = posEnemy.z - posPlayer.z;
                float radarX = diffX / radar_radius * GetComponent<RectTransform>().rect.width / 2;
                float radarY = diffY / radar_radius * GetComponent<RectTransform>().rect.height / 2;
                Vector2 radarPos = Vector2.zero;
                radarPos.Set(radarX, radarY);

                GameObject radarDot = Instantiate(dot, gameObject.transform);
                radarDot.GetComponent<RectTransform>().anchoredPosition += radarPos;
                Destroy(radarDot, 3.0f);

                if (!detected)
                    GetComponent<AudioSource>().Play();

                detected = true;
            }
        }

        return detected;
    }
}
