using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject[] enemyPrefabs;
    public GameObject[] powerupPrefab;

    private float spawnRange = 9.0f;
    public int enemyCount;
    public int waveNumber = 1;

    // Start is called before the first frame update
    void Start()
    {
        SpawnEnemyWaves(waveNumber);
        SpawnPowerup();
    }

    // Update is called once per frame
    void Update()
    {
        enemyCount = FindObjectsOfType<Enemy>().Length;
        if(enemyCount == 0)
        {
            waveNumber++;
            if (waveNumber % 4 != 0)
            {
                SpawnEnemyWaves(waveNumber);
            }
            else
            {
                SpawnBoss();
            }

            SpawnPowerup();
            
        }
    }

    void SpawnEnemyWaves(int enemiesToSpawn)
    {
        for (int i = 0; i < enemiesToSpawn ; i++)
        {
            int enemyRandomizer = Random.Range(0, 10);
            if (enemyRandomizer < 2)
            {
                Instantiate(enemyPrefabs[1], GenerateSpawnPosition(), enemyPrefabs[1].transform.rotation);
            }
            else
            {
                Instantiate(enemyPrefabs[0], GenerateSpawnPosition(), enemyPrefabs[0].transform.rotation);
            }            
        }
    }

    void SpawnBoss()
    {
        Instantiate(enemyPrefabs[2], GenerateSpawnPosition(), enemyPrefabs[2].transform.rotation);
    }

    void SpawnPowerup()
    {
        int powerupIndex = Random.Range(0, powerupPrefab.Length);
        Instantiate(powerupPrefab[powerupIndex], GenerateSpawnPosition(), powerupPrefab[powerupIndex].transform.rotation);
    }

    Vector3 GenerateSpawnPosition()
    {
        float spawnPosX = Random.Range(-spawnRange, spawnRange);
        float spawnPosZ = Random.Range(-spawnRange, spawnRange);

        Vector3 randomPos = new Vector3(spawnPosX, 0, spawnPosZ);

        return randomPos;
    }
}
