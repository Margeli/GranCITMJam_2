using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeToBlack : MonoBehaviour
{

    private Image image;
    public bool fade = false;
    public bool finished = false;
    
    void Start()
    {
        image = GetComponent<Image>();
    }
    
    void Update()
    {
        if (fade)
        {
            image.color = new Color(image.color.r, image.color.g, image.color.b, image.color.a + 0.01f);

            if (image.color.a >= 1.0f)
                finished = true;
        }
    }
}
