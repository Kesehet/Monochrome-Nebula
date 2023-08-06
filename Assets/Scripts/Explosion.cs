using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    public GameObject prefab; // assign your prefab in the inspector
    public int numberOfInstances = 5; // number of instances to create
    public float spawnRadius = 0.5f; // radius around the object where the prefabs will be instantiated
    public bool shouldExplode = true; // should the instantiated object also explode?

    
    public void LoadCluster(){
            if(shouldExplode){   
            // Instantiate the prefab multiple times
            for (int i = 0; i < numberOfInstances; i++)
            {
                // Calculate a random position within the spawn radius
                Vector2 spawnPosition = transform.position + new Vector3(Random.Range(-spawnRadius, spawnRadius), Random.Range(-spawnRadius, spawnRadius), 0);

                // Instantiate the prefab at the calculated position
                GameObject instance = Instantiate(prefab, spawnPosition, Quaternion.identity);

                // Randomize the scale of the instantiated prefab
                float scale = Random.Range(0.1f, 0.5f); // adjust the range as needed
                instance.transform.localScale = new Vector3(scale, scale, scale);

                // If the instantiated object should not explode, disable the Explosion script
                
                Explosion explosion = instance.GetComponent<Explosion>();
                if (explosion != null)
                {
                    explosion.shouldExplode = false;
                }
            
            }
        }
    }

    public void Disappear()
    {
        // Destroy the current game object
        Destroy(gameObject);
    }
}
