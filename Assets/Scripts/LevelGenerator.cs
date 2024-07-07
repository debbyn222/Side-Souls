using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class LevelGenerator : MonoBehaviour
{
    public List<Segment> segmentPrefabs;  // List of all segment prefabs
    public Transform player;
    public Transform startPoint;
    private float generationDistance = 10f; // Distance ahead of the player to generate new segments
    private float despawnDistance = 100f; // Distance behind the player to despawn old segments

    private List<GameObject> activeSegments = new List<GameObject>();
    private Vector3 nextSpawnPosition;
    private Transform lastExitPoint;
    private GameObject lastSegment;
    private List<GameObject> despawnedSegments = new List<GameObject>();

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
            //Debug.Log("player distance from current segment position: " + Vector3.Distance(player.position, segment.transform.position));
            if (segment != lastSegment && Vector3.Distance(player.position, segment.transform.position) > despawnDistance)
            {
                segmentsToDespawn.Add(segment);
                despawnedSegments.Add(segment);

            }
        }

        foreach (GameObject segment in segmentsToDespawn)
        {
            activeSegments.Remove(segment);
            segment.SetActive(false);
        }
    }

    void RespawnOldSegments()
    {
        List<GameObject> segmentsToRespawn = new List<GameObject>();

        foreach( GameObject seg in despawnedSegments)
        {
            if(Vector3.Distance(player.position, seg.transform.position) < despawnDistance)
            {
                segmentsToRespawn.Add(seg);
            }
        }

        foreach (GameObject seg in segmentsToRespawn)
        {

            activeSegments.Add(seg);
            despawnedSegments.Remove(seg);
            seg.SetActive(true);
        }
    }




}