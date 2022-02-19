using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [SerializeField] GameObject EnemyPrefab;
    [SerializeField][Range(0.1f, 30f)] float spawnTimer = 1f;
    [SerializeField][Range(0,50)] int poolSize = 5;

    GameObject[] pool;

    void Awake() {

        PopulatePool();    
    }

    void Start() 
    {
        StartCoroutine(SpawnEnemy());
    }

    void PopulatePool()
    {
        pool = new GameObject[poolSize];

        for (int i = 0; i < pool.Length; i++)
        {
            pool[i] = Instantiate(EnemyPrefab,transform);
            pool[i].SetActive(false); 
            // Debug.Log("Set active to false");
        }    
    }

    void EnableObjectInPool(){

        for(int i=0; i < pool.Length; i++)
        {
            GameObject enemy = pool[i];
            // Debug.Log($"Enable enemy at index {enemy.name}");
            if(!pool[i].activeInHierarchy)
            {
                // Debug.Log($"set active to true {i}");

                pool[i].SetActive(true);
                return;//Return early

            }

        }

    }

    IEnumerator SpawnEnemy()
    {   
        while(true)
        {
            EnableObjectInPool();
            yield return new WaitForSeconds(spawnTimer);
        }

    }
}
