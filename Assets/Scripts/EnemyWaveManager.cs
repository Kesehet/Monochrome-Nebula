using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWaveManager : MonoBehaviour
{
    public List<GameObject> enemyPrefabs; // List of enemy prefabs
    public List<Transform> spawnPoints; // List of spawn points
    public int initialWaveSize = 5; // Number of enemies in the first wave
    public int waveIncrement = 5; // How many more enemies to add each wave
    public float waveDelay = 5f; // Time to wait between waves

    private List<GameObject> currentWave; // List of enemies in the current wave
    private int waveNumber = 0; // Current wave number

    void Start()
    {
        currentWave = new List<GameObject>();
        StartCoroutine(SpawnWave());
    }

    IEnumerator SpawnWave()
    {
        waveNumber++;
        int waveSize = initialWaveSize + (waveNumber - 1) * waveIncrement;

        for (int i = 0; i < waveSize; i++)
        {
            // Randomly select a spawn point from the list
            Transform selectedSpawnPoint = spawnPoints[Random.Range(0, spawnPoints.Count)];

            // Instantiate a random enemy from the list of enemy prefabs
            GameObject enemyPrefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Count)];
            GameObject enemy = Instantiate(enemyPrefab, selectedSpawnPoint.position, selectedSpawnPoint.rotation);
            currentWave.Add(enemy);

            // Wait a short time before spawning the next enemy
            yield return new WaitForSeconds(0.5f);
        }

        // Wait for all enemies in the current wave to die
        while (currentWave.Exists(enemy => enemy != null))
        {
            yield return null;
        }

        // Wait for the delay before starting the next wave
        yield return new WaitForSeconds(waveDelay);

        // Start the next wave
        StartCoroutine(SpawnWave());
    }
}
