using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class LevelGenerator : MonoBehaviour
{
    public List<Segment> segmentPrefabs;  // List of all segment prefabs
    public Transform player;
    public Transform startPoint;
    private float generationDistance = 10f; // Distance ahead of the player to generate new segments
    private float despawnDistance = 30f; // Distance behind the player to despawn old segments

    private List<GameObject> activeSegments = new List<GameObject>();
    private Vector3 nextSpawnPosition;
    private Transform lastExitPoint;
    private GameObject lastSegment;

    // Dictionary to store information about despawned segments
    private Dictionary<Vector3, Segment> despawnedSegments = new Dictionary<Vector3, Segment>();

    void Start()
    {
        nextSpawnPosition = startPoint.position;
        GenerateInitialSegment();
    }

    void Update()
    {
        if (Vector3.Distance(player.position, nextSpawnPosition) < generationDistance)
        {
            GenerateSegment();
        }

        DespawnOldSegments();
        RespawnOldSegments();
    }

    void GenerateSegment()
    {
        Segment segment;
        /*
                // Check if there is a despawned segment that needs to be respawned
                if (despawnedSegments.TryGetValue(nextSpawnPosition, out segment))
                {
                    despawnedSegments.Remove(nextSpawnPosition);
                }
                else
                {
                    segment = segmentPrefabs[Random.Range(1, segmentPrefabs.Count)];
                }*/
        segment = segmentPrefabs[Random.Range(1, segmentPrefabs.Count)];
        GameObject newSegment = Instantiate(segment.prefab);

        if (lastExitPoint != null)
        {
            newSegment.transform.position = lastExitPoint.position - segment.entryPoint.localPosition;
            

        }
        else
        {
            newSegment.transform.position = nextSpawnPosition;
            Debug.Log("last exit point null");
        }





        activeSegments.Add(newSegment);
        lastSegment = newSegment;
        lastExitPoint = newSegment.transform.Find("ExitPoint");

        // Adjust the next spawn position
        nextSpawnPosition = lastExitPoint.position;
        Debug.DrawLine(nextSpawnPosition, nextSpawnPosition + new Vector3(0, 1, 0), Color.red, 50f);
    }

    void GenerateInitialSegment()
    {
        Segment initialSegment = segmentPrefabs[0]; // The first prefab is the spawn prefab
        GameObject newSegment = Instantiate(initialSegment.prefab);

        newSegment.transform.position = nextSpawnPosition;
        activeSegments.Add(newSegment);

        lastSegment = newSegment;
        lastExitPoint = newSegment.transform.Find("ExitPoint");
        nextSpawnPosition = lastExitPoint.position;


    }

    void DespawnOldSegments()
    {
        if (activeSegments.Count == 0) return;

        List<GameObject> segmentsToDespawn = new List<GameObject>();

        foreach (GameObject segment in activeSegments)
        {
            if (segment != lastSegment && Vector3.Distance(player.position, segment.transform.position) > despawnDistance)
            {
                segmentsToDespawn.Add(segment);
                Segment segmentData = segmentPrefabs.FirstOrDefault(s => s.prefab.name == segment.name.Replace("(Clone)", "").Trim());
                despawnedSegments[segment.transform.position] = segmentData;
            }
        }

        foreach (GameObject segment in segmentsToDespawn)
        {
            activeSegments.Remove(segment);
            Destroy(segment);
        }
    }

    void RespawnOldSegments()
    {
        List<Vector3> segmentsToRespawn = new List<Vector3>();

        foreach (KeyValuePair<Vector3, Segment> kvp in despawnedSegments)
        {
            if (Vector3.Distance(player.position, kvp.Key) < despawnDistance)
            {
                segmentsToRespawn.Add(kvp.Key);
            }
        }

        foreach (Vector3 position in segmentsToRespawn)
        {
            Segment segment = despawnedSegments[position];
            //GameObject newSegment = Instantiate(segment.prefab, position, Quaternion.identity);
            GameObject newSegment = Instantiate(segment.prefab);
            activeSegments.Add(newSegment);
            despawnedSegments.Remove(position);
            newSegment.transform.position = position;
        }
    }
}