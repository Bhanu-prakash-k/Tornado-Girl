using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPrefabGrid : MonoBehaviour
{
    public GameObject[] prefabs;
    public int gridX;
    public int gridZ;
    public float gridSpacingOffset = 1f;
    public Vector3 gridOrigin = Vector3.zero;
    public Vector3 positionRandomization;
    
    // Start is called before the first frame update
    void Start()
    {
        SpawnGrid();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void SpawnGrid()
    {
        for (int x = 0; x < gridX; x++)
        {
            for(int z = 0; z < gridZ; z++)
            {
                Vector3 spawnPosition = new Vector3(x * gridSpacingOffset, 0, z * gridSpacingOffset) + gridOrigin;
                PickAndSpawn(RandomizePosition(spawnPosition), Quaternion.identity);
            }
        }
    }

    Vector3 RandomizePosition(Vector3 position)
    {
        Vector3 randomizedPosition = new Vector3(Random.Range(-positionRandomization.x, positionRandomization.x), Random.Range(-positionRandomization.y, positionRandomization.y), Random.Range(-positionRandomization.z, positionRandomization.z)) + position;

        return randomizedPosition;
    }

    void PickAndSpawn(Vector3 positionToTransform, Quaternion rotationToTransform)
    {
        int randomIndex = Random.Range(0, prefabs.Length);
        GameObject clone = Instantiate(prefabs[randomIndex], positionToTransform, rotationToTransform);

    }
}
