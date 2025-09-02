using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RifleSpawner : MonoBehaviour
{
    [SerializeField] private PlayerController player;
    [SerializeField] private int[] rifleSpawnPositionX = new int[3];
    [SerializeField] private float rifleSpawnPositionY = 2f;
    [SerializeField] private float spawnDistanceAhead = 20f;
    [SerializeField] private float spawnRangeZ = 5f;
    // [SerializeField] private GameObject riflePrefab;
    [SerializeField] private float spawnTime;
    
    [SerializeField] private RiflePool riflePool;

    private List<RifleController> spawnedRifles = new List<RifleController>();

    private void Start()
    {
        StartCoroutine(SpawnRifle());
    }


    IEnumerator SpawnRifle()
    {
        while (!player.isDead)
        {
            int randomXIndex = Random.Range(0, rifleSpawnPositionX.Length);
            float randomZ = Random.Range(-spawnRangeZ, spawnRangeZ);

            float spawnZ = player.transform.position.z + spawnDistanceAhead + randomZ;
            float spawnX = rifleSpawnPositionX[randomXIndex];

            Vector3 spawnPosition = new Vector3(spawnX, rifleSpawnPositionY, spawnZ);

            // GameObject rifle = Instantiate(riflePrefab, spawnPosition, Quaternion.Euler(0, 180, 0));
            RifleController rifle = riflePool.Get();
            rifle.transform.SetPositionAndRotation(spawnPosition, Quaternion.Euler(0, 180, 0));
            rifle.AssignSpawner(this);
            // rifle.transform.SetParent(transform);

            // RifleController rifleScript = rifle.GetComponent<RifleController>();
            spawnedRifles.Add(rifle);

            yield return new WaitForSeconds(spawnTime);
        }
    }

    public void DestroyAllRifles(RifleController excludeRifle)
    {
        
        for (int i = spawnedRifles.Count - 1; i >= 0; i--)
        {
            RifleController rifle = spawnedRifles[i];

            if (rifle != null && rifle != excludeRifle)
            {
                ReturnRifleToPool(rifle);
            }
        }
    }

    public void ReturnRifleToPool(RifleController rifle)
    {
        riflePool.Release(rifle);
        spawnedRifles.Remove(rifle);
    }
}