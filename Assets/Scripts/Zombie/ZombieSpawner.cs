using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieSpawner : MonoBehaviour
{
    [SerializeField] private PlayerController player;
    [SerializeField] private int[] zombieSpawnPositionX = new int[3];
    [SerializeField] private float spawnDistanceAhead = 20f;
    [SerializeField] private float spawnRangeZ = 5f;
    // [SerializeField] private GameObject zombiePrefab;
    [SerializeField] private float spawnTime;
    
    [SerializeField] private ZombiePool zombiePool;

    private List<ZombieController> spawnedZombies = new List<ZombieController>();
    private void Start()
    {
        StartCoroutine(SpawnZombie());
    }

    IEnumerator SpawnZombie()
    {

        while (!player.isDead)
        {
            int randomXIndex = Random.Range(0, zombieSpawnPositionX.Length);
            float randomZ = Random.Range(-spawnRangeZ, spawnRangeZ);

            float spawnZ = player.transform.position.z + spawnDistanceAhead + randomZ;

            Vector3 spawnPosition = new Vector3(zombieSpawnPositionX[randomXIndex], 0, spawnZ);
            // GameObject zombie = Instantiate(zombiePrefab, spawnPosition, Quaternion.Euler(0, 180, 0));
            ZombieController zombie = zombiePool.Get();
            zombie.transform.SetPositionAndRotation(spawnPosition, Quaternion.Euler(0, 180, 0));
            // zombie.transform.SetParent(transform);

            // ZombieController zombieScript = zombie.GetComponent<ZombieController>();
            spawnedZombies.Add(zombie);

            yield return new WaitForSeconds(spawnTime);
        }
    }

    public void DestroyOtherZombies(ZombieController excludeZombie)
    {
        // foreach (ZombieController zombie in spawnedZombies)
        // {
        //     if (zombie != null && zombie != excludeZombie)
        //     {
        //         // Destroy(zombie.gameObject);
        //         spawnedZombies.Remove(zombie);
        //         zombiePool.Release(zombie);
        //     }
        // }
        
        for (int i = spawnedZombies.Count - 1; i >= 0; i--)
        {
            ZombieController zombie = spawnedZombies[i];

            if (zombie != null && zombie != excludeZombie)
            {
                zombiePool.Release(zombie);
                spawnedZombies.RemoveAt(i);
            }
        }


        // spawnedZombies.Clear();
        // spawnedZombies.Add(excludeZombie);
    }

}