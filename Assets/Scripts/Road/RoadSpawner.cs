using System.Collections.Generic;
using UnityEngine;

public class RoadSpawner : MonoBehaviour
{
    private float roadSpawnZ;
    private Queue<Road> activeRoads = new Queue<Road>();
    
    [SerializeField] private float roadSpawnX = -3.3f;

    [SerializeField] private RoadPool roadPool;   

    [SerializeField] private int firstRoadSpawnPosition = 40;
    [SerializeField] private int roadsOnScreen = 10;
    [SerializeField] private float despawnDistance = 5f;

    [SerializeField] private Transform cameraTransform;

    private void Awake()
    {
        ResetWorld();
    }

    private void Update()
    {
        ScanPosition();
    }

    public void ScanPosition()
    {
        float cameraZ = cameraTransform.position.z;
        Road firstRoad = activeRoads.Peek();

        if (cameraZ >= firstRoad.transform.position.z + firstRoad.roadLength + despawnDistance)
        {
            SpawnNewRoad();
            DeleteOldRoad();
        }
    }

    private void SpawnNewRoad()
    {
        Road road = roadPool.Get();
        road.transform.position = new Vector3(roadSpawnX, 0, roadSpawnZ);
        roadSpawnZ += road.roadLength;

        activeRoads.Enqueue(road);
    }

    private void DeleteOldRoad()
    {
        Road road = activeRoads.Dequeue();
        roadPool.Release(road);
    }

    private void ResetWorld()
    {
        roadSpawnZ = firstRoadSpawnPosition;

        while (activeRoads.Count > 0)
        {
            DeleteOldRoad();
        }

        for (int i = 0; i < roadsOnScreen; i++)
        {
            SpawnNewRoad();
        }
    }
}



// using System.Collections.Generic;
// using UnityEngine;
//
// public class RoadGeneration : MonoBehaviour
// {
//     private float roadSpawnZ;
//     [SerializeField] private float roadSpawnX = -3.3f;
//     
//     
//     private Queue<Road> activeRoads = new Queue<Road>();
//     private Queue<Road> roadPool = new Queue<Road>();  
//     
//
//     [SerializeField] private int firstRoadSpawnPosition = 40;
//     [SerializeField] private int roadsOnScreen = 10;
//     [SerializeField] private float despawnDistance = 5f;
//
//     [SerializeField] private List<GameObject> roadPrefab;
//     [SerializeField] private Transform cameraTransform;
//
//
//     private void Awake()
//     {
//         ResetWorld();
//     }
//
//     // private void Start()
//     // {
//     //     if (roadPrefab == null)
//     //     {
//     //         Debug.LogError("Road Prefab not assigned");
//     //         return;
//     //     }
//     //
//     // }
//     
//     private void Update()
//     {
//         ScanPosition();
//     }
//
//     public void ScanPosition()
//     {
//         float cameraZ = cameraTransform.position.z;
//         Road lastRoad = activeRoads.Peek();
//
//         if (cameraZ >= lastRoad.transform.position.z + lastRoad.roadLength + despawnDistance)
//         {
//             SpawnNewRoads();
//             DeleteLastRoads();
//         }
//     }
//
//     public void SpawnNewRoads()
//     {
//         int randomIndex = Random.Range(0, roadPrefab.Count);
//         Road road;
//
//         if (roadPool.Count > 0)
//         {
//             road = roadPool.Dequeue();
//             road.gameObject.SetActive(true);
//         }
//         else
//         {
//             GameObject newRoad = Instantiate(roadPrefab[randomIndex], transform);
//             road = newRoad.GetComponent<Road>();
//         }
//
//         road.transform.position = new Vector3(roadSpawnX, 0, roadSpawnZ);
//         roadSpawnZ += road.roadLength;
//
//         activeRoads.Enqueue(road);
//     }
//
//     public void DeleteLastRoads()
//     {
//         Road road = activeRoads.Dequeue();
//         road.gameObject.SetActive(false);   // Instead of destroying
//         roadPool.Enqueue(road);   
//     }
//
//
//     public void ResetWorld()
//     {
//         roadSpawnZ = firstRoadSpawnPosition;
//
//         while (activeRoads.Count > 0)
//         {
//             DeleteLastRoads();
//         }
//         
//         //spawn initial roads
//         for (int i = 0; i < roadsOnScreen; i++)
//         {
//             SpawnNewRoads();
//         }
//
//     }
// }