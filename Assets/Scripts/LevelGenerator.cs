using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class LevelGenerator : MonoBehaviour
{
    public List<Segment> segments;  // List of available segments
    public Transform player;
    public Camera mainCamera;
    public float generationOffset = 5f; // Offset before the camera edge where new segments are generated
    public Transform startPoint;

    private Vector2 currentPoint;
    private Dictionary<Vector2, GameObject> generatedSegments = new Dictionary<Vector2, GameObject>();

    void Start()
    {
        currentPoint = startPoint.position;
        GenerateInitialSegments();
    }

    void Update()
    {
        Vector3 cameraRightEdge = mainCamera.ViewportToWorldPoint(new Vector3(1, 0.5f, 0));
        if (cameraRightEdge.x > currentPoint.x - generationOffset)
        {
            GenerateSegment();
        }
    }

    void GenerateInitialSegments()
    {
        GameObject spawnPrefab = segments[0].prefab;//ensure spawn prefab is first in segment list
        if (spawnPrefab != null)
        {
            GameObject spawnInstance = Instantiate(spawnPrefab, currentPoint, Quaternion.identity);
            float spawnLength = CalculateSegmentLength(spawnInstance);
            //float spawnLength = segments[0].exitPoint.position.x - segments[0].entryPoint.position.x;
            currentPoint += new Vector2(spawnLength, 0);
        }

        for (int i = 0; i < 3; i++)
        { // Generate a few initial segments
            GenerateSegment();
        }
    }

    void GenerateSegment()
    {
        Vector2 segmentPosition = new Vector2(currentPoint.x, currentPoint.y);

        if (!generatedSegments.ContainsKey(segmentPosition))
        {
            Segment segment = segments[Random.Range(1, segments.Count)];//start at 1 to excluse spawn
            GameObject segmentInstance = Instantiate(segment.prefab, currentPoint, Quaternion.identity);
            generatedSegments[segmentPosition] = segmentInstance;

            // Calculate the segment length dynamically
            float segmentLength = segment.exitPoint.position.x - segment.entryPoint.position.x;
            Debug.Log(segmentLength);
            currentPoint += new Vector2(segmentLength, 0);
        }
        else
        {
            float segmentLength = CalculateSegmentLength(generatedSegments[segmentPosition]);
            currentPoint += new Vector2(segmentLength, 0);
        }
    }


    /*    void GenerateSegment()
        {
            Vector2 segmentPosition = new Vector2(currentPoint.x, currentPoint.y);

            if (!generatedSegments.ContainsKey(segmentPosition))
            {
                Segment segment = segments[Random.Range(1, segments.Count)]; // Start at 1 to exclude spawn
                GameObject segmentInstance = Instantiate(segment.prefab, currentPoint, Quaternion.identity);
                generatedSegments[segmentPosition] = segmentInstance;

                // Adjust the current point to the exit point of the newly instantiated segment
                Transform exitPoint = segmentInstance.transform.Find("ExitPoint");
                if (exitPoint != null)
                {
                    currentPoint = new Vector2(exitPoint.position.x, exitPoint.position.y);
                }
                else
                {
                    Debug.LogError("ExitPoint not found in segment prefab: " + segment.prefab.name);
                }
            }
            else
            {
                float segmentLength = CalculateSegmentLength(generatedSegments[segmentPosition]);
                currentPoint += new Vector2(segmentLength, 0);
            }
        }*/


    float CalculateSegmentLength(GameObject segmentInstance)
    {
        // Assuming the segment has a BoxCollider2D or Renderer to determine its length
        BoxCollider2D collider = segmentInstance.GetComponent<BoxCollider2D>();
        if (collider != null)
        {
            return collider.size.x;
        }

        Renderer renderer = segmentInstance.GetComponent<Renderer>();
        if (renderer != null)
        {
            return renderer.bounds.size.x;
        }

        // Fallback if no collider or renderer is found
        return segmentInstance.transform.localScale.x;
    }
}
