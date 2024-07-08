using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ParallaxEffect : MonoBehaviour
{
    private Transform startPoint; // Reference to the start point transform
    private float length, startpos; // Variables to store the length of the sprite and its initial position
    private GameObject cam; // Reference to the main camera
    public float parallaxEffect; // Parallax effect multiplier, should be negative for reversing direction

    void Start()
    {
        // Find the main camera and start point objects in the scene
        cam = GameObject.Find("Main Camera");
        startPoint = GameObject.Find("StartPoint").transform;

        // Set the initial start position based on the start point
        startpos = startPoint.position.x;

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
        transform.position = new Vector3(startpos + dist, transform.position.y, transform.position.z);

        // Adjust start position to create a seamless loop effect
        if (temp > startpos + length)
            startpos += length;
        else if (temp < startpos - length)
            startpos -= length;
    }
}
