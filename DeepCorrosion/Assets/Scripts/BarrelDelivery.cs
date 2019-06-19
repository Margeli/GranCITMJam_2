using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrelDelivery : MonoBehaviour
{
    public GameObject barrel1;
    public GameObject barrel2;
    public GameObject barrel3;
    public GameObject barrel4;
    public uint delivered_barrels = 0;
    
    public void SetBarrel()
    {
        delivered_barrels++;

        if (delivered_barrels == 0)
        {
            barrel1.SetActive(false);
            barrel2.SetActive(false);
            barrel3.SetActive(false);
            barrel4.SetActive(false);
        }
        if (delivered_barrels == 1)
        {
            barrel1.SetActive(true);
            barrel2.SetActive(false);
            barrel3.SetActive(false);
            barrel4.SetActive(false);
        }
        if (delivered_barrels == 2)
        {
            barrel1.SetActive(true);
            barrel2.SetActive(true);
            barrel3.SetActive(false);
            barrel4.SetActive(false);
        }
        if (delivered_barrels == 3)
        {
            barrel1.SetActive(true);
            barrel2.SetActive(true);
            barrel3.SetActive(true);
            barrel4.SetActive(false);
        }
        if (delivered_barrels == 4)
        {
            barrel1.SetActive(true);
            barrel2.SetActive(true);
            barrel3.SetActive(true);
            barrel4.SetActive(true);
        }
    }
}
