using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class LevelGenerator : MonoBehaviour
{
    public List<Segment> segmentPrefabs;  // List of all segment prefabs
    public Transform player;              
    public Transform startPoint;
    private float generationDistance = 10f; // Distance ahead of the player to generate new segments
    private float despawnDistance = 30f;   // Distance behind the player to despawn old segments

    private List<GameObject> activeSegments = new List<GameObject>();
    private Vector3 nextSpawnPosition;
    private Transform lastExitPoint;      

    void Start()
    {
        nextSpawnPosition = startPoint.position;
        GenerateInitialSegment();
        //GenerateSegment(); 
    }

    void Update()
    {
        
        if (Vector3.Distance(player.position, nextSpawnPosition) < generationDistance)
        {
            GenerateSegment();
        }

        DespawnOldSegments();
    }

    void GenerateSegment()
    {
        Segment segment = segmentPrefabs[Random.Range(1, segmentPrefabs.Count)];
        GameObject newSegment = Instantiate(segment.prefab);

        if (lastExitPoint != null)
        {
            newSegment.transform.position = lastExitPoint.position - segment.entryPoint.localPosition;
        }
        else
        {
            newSegment.transform.position = nextSpawnPosition;
        }

        activeSegments.Add(newSegment);

        lastExitPoint = newSegment.transform.Find("ExitPoint");

        // Adjust the next spawn position
        nextSpawnPosition = lastExitPoint.position;

        // Debugging: Visualize the spawn points
        //Debug.DrawLine(newSegment.transform.position, nextSpawnPosition, Color.red, 20f);
    }

    void GenerateInitialSegment()
    {
        Segment initialSegment = segmentPrefabs[0]; // The first prefab is the spawn prefab
        GameObject newSegment = Instantiate(initialSegment.prefab);

        newSegment.transform.position = nextSpawnPosition;
        activeSegments.Add(newSegment);

        // Find and set the next spawn position using the exit point
        lastExitPoint = newSegment.transform.Find("ExitPoint");
        nextSpawnPosition = lastExitPoint.position;

        // Draw debug spheres at the entry and exit points for visual confirmation
        //DrawDebugSpheres(newSegment);

        // Debug line from the current segment's position to the next spawn position
        Debug.DrawLine(newSegment.transform.position, nextSpawnPosition, Color.red, 10f);
    }


    void DespawnOldSegments()
    {
        if (activeSegments.Count == 0) return;

        List<GameObject> segmentsToDespawn = new List<GameObject>();

        foreach (GameObject segment in activeSegments)
        {
            if (Vector3.Distance(player.position, segment.transform.position) > despawnDistance)
            {
                segmentsToDespawn.Add(segment);
            }
        }

        foreach (GameObject segment in segmentsToDespawn)
        {
            activeSegments.Remove(segment);
            Destroy(segment);
        }
    }
}
