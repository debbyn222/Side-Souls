using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ParallaxEffect : MonoBehaviour
{
    private Transform startPoint;
    private float length;
    private GameObject cam;
    public float parallaxEffect; // Parallax effect multiplier, should be negative for reversing direction


    void Start()
    {
        cam = GameObject.Find("Main Camera");
        startPoint = transform.parent.Find("StartPosition");

        // Get the length of the sprite for seamless looping
        length = GetComponent<SpriteRenderer>().bounds.size.x;
        parallaxEffect = parallaxEffect * -1;

    }

    void Update()
    {

        // Calculate temporary position based on camera's position and inverse of parallax effect
        float temp = (cam.transform.position.x * (1 - parallaxEffect));

        // Calculate distance the background should move
        float dist = (cam.transform.position.x * parallaxEffect);

        // Update the background position based on the calculated distance
        transform.position = new Vector3(startPoint.position.x + dist, transform.position.y, transform.position.z);

    }

}