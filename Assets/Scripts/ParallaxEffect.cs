using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ParallaxEffect : MonoBehaviour
{
    private float length, startpos;
    private GameObject cam;
    public float parallaxEffect; // Parallax effect multiplier, should be negative for reversing direction
    private Transform segmentTransform; // Reference to the segment prefab
    private float segmentMinX, segmentMaxX; // Segment bounds
    private bool initialized = false;

    void Start()
    {
        cam = GameObject.Find("Main Camera");
        segmentTransform = transform.parent; // Assuming the background is a child of the segment prefab

        // Get the length of the sprite for seamless looping
        length = GetComponent<SpriteRenderer>().bounds.size.x;
        parallaxEffect = parallaxEffect * -1;

        // Start the coroutine to initialize the parallax effect
        StartCoroutine(InitializeParallaxEffect());
    }

    void Update()
    {
        if (!initialized)
            return;

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

        // Check if the background moves beyond the segment bounds and destroy it if it does
        CheckBackgroundBounds();
    }

    IEnumerator InitializeParallaxEffect()
    {
        while (!initialized)
        {
            GameObject entryPointObj = GameObject.FindGameObjectWithTag("EntryPoint");
            GameObject exitPointObj = GameObject.FindGameObjectWithTag("ExitPoint");

            if (entryPointObj != null && exitPointObj != null)
            {
                Transform entryPoint = entryPointObj.transform;
                Transform exitPoint = exitPointObj.transform;
                segmentMinX = entryPoint.position.x;
                segmentMaxX = exitPoint.position.x;
                startpos = entryPoint.position.x; // Initialize start position based on entry point
                initialized = true;
                Debug.Log($"ParallaxEffect initialized: EntryPoint at {segmentMinX}, ExitPoint at {segmentMaxX}");
            }
            else
            {
                Debug.LogWarning("EntryPoint or ExitPoint not found, retrying...");
                yield return new WaitForSeconds(0.1f); // Wait before retrying
            }
        }
    }

    void CheckBackgroundBounds()
    {
        if (!initialized)
            return;

        if (transform.position.x < segmentMinX || transform.position.x > segmentMaxX)
        {
            Debug.Log($"Background out of bounds: {transform.position.x}");
            Destroy(gameObject);
        }
    }
}