using System.Collections.Generic;
using UnityEngine;

public class RoadGeneration : MonoBehaviour
{
    private float roadSpawnZ;
    [SerializeField] private float roadSpawnX = -3.3f;
    private Queue<Road> activeRoads = new Queue<Road>();
    //private List<Road> roadPool = new List<Road>();

    [SerializeField] private int firstRoadSpawnPosition = 40;
    [SerializeField] private int roadsOnScreen = 10;
    [SerializeField] private float despawnDistance = 5f;

    [SerializeField] private List<GameObject> roadPrefab;

    //[SerializeField] private GameObject roadPrefab;

    [SerializeField] private Transform cameraTransform;


    private void Awake()
    {
        ResetWorld();
    }

    private void Start()
    {
        if (roadPrefab == null)
        {
            Debug.LogError("Road Prefab not assigned");
            return;
        }

    }
    private void Update()
    {
        ScanPosition();
    }

    public void ScanPosition()
    {
        float cameraZ = cameraTransform.position.z;
        Road lastRoad = activeRoads.Peek();

        if (cameraZ >= lastRoad.transform.position.z + lastRoad.roadLength + despawnDistance)
        {
            SpawnNewRoads();
            DeleteLastRoads();
        }
    }

    public void SpawnNewRoads()
    {
        int randomIndex = Random.Range(0, roadPrefab.Count);
        Road road = null;

        if (road == null)
        {
            //GameObject newRoad = Instantiate(roadPrefab, transform);
            GameObject newRoad = Instantiate(roadPrefab[randomIndex], transform);

            road = newRoad.GetComponent<Road>();
        }

        road.transform.position = new Vector3(roadSpawnX, 0, roadSpawnZ);
        roadSpawnZ += road.roadLength;

        activeRoads.Enqueue(road);
    }

    public void DeleteLastRoads()
    {
        Road road = activeRoads.Dequeue();
        road.DestroyRoad();
        //roadPool.Add(road);
    }


    public void ResetWorld()
    {
        roadSpawnZ = firstRoadSpawnPosition;

        for (int i = activeRoads.Count; i != 0; i--)
        {
            DeleteLastRoads();
        }

        for (int i = 0; i < roadsOnScreen; i++)
        {
            SpawnNewRoads();
        }

    }
}
