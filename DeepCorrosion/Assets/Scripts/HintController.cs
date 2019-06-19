using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HintController : MonoBehaviour
{

    public string object_name;
    private GameObject gobject;

    private void Start()
    {
        gobject = GameObject.Find(object_name);
        gobject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
            gobject.SetActive(true);

        Invoke("Disable", 5.0f);
    }

    private void Disable()
    {
        Destroy(gobject);
        Destroy(this.gameObject);
    }
}
