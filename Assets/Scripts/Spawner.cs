using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject enemies;
    public float xPos;
    public float zPos;
    public int enemyCount;
    public int enemySpawnCount;

    [SerializeField] private float xMin, xMax;
    [SerializeField] private float zMin, zMax;
    
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(EnemyDrop());
    }
    IEnumerator EnemyDrop()
    {
        while(enemyCount < enemySpawnCount)
        {
            xPos = Random.Range(xMin, xMax);
            zPos = Random.Range(zMin, zMax);
            Instantiate(enemies, new Vector3(xPos, 0, zPos), Quaternion.identity);
            yield return new WaitForSeconds(0.1f);
            enemyCount += 1;
        }
    }
}
