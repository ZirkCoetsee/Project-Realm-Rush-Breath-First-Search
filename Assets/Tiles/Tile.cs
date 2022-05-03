using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class Tile : MonoBehaviour
{
    [SerializeField] Tower TowerPrefab;
    [SerializeField] bool isPlacable;

    [SerializeField] bool hasObject;

    [SerializeField] bool willBlock;

    public TreePrefabWeighted [] TreePrefabs;

    private float[] treeWeights;

    // Property for access from other classes
    public bool IsPlacable{

        get
        {
            return isPlacable;
        }
    }

    GridManager gridManager;
    Pathfinder pathFinder;

    Bank bank;
    Vector2Int coordinates = new Vector2Int();
    public Vector2Int Coordinates { get {return coordinates; } }
    private void Awake() 
    {    
        gridManager = FindObjectOfType<GridManager>();
        pathFinder = FindObjectOfType<Pathfinder>();
        bank = FindObjectOfType<Bank>();


    }

    void Start()
    {
        treeWeights = TreePrefabs.Select(prefabStats => prefabStats.weight).ToArray();



        if(gridManager != null){
            coordinates = gridManager.GetCoordinatesFromPostion(transform.position);
            if(!isPlacable)
            {
                gridManager.BlockNode(coordinates);
            }else
            {
                bool willBlockPath = pathFinder.WillBlockPath(coordinates);
                if(RandomSpawn() 
                && pathFinder.StartCoordinates != coordinates 
                && pathFinder.EndCoordinates != coordinates
                && !hasObject && !willBlock && !willBlockPath){
                    gridManager.BlockNode(coordinates);
                    // int randomIndex = GetRandomWeightedIndex(treeWeights);
                    // Debug.Log("Random Index: "+ randomIndex);
                    var euler = transform.eulerAngles;
                    euler.y = UnityEngine.Random.Range(0f,360f);
                    int randomInt = UnityEngine.Random.Range(1,treeWeights.Count());
                    TreePrefabs[randomInt].prefab.transform.eulerAngles = euler;
                    SpawnObject(TreePrefabs[randomInt].prefab,transform.position);

                }
            }
        }
    }

    // void OnMouseDown(){

    //     if(gridManager.GetNode(coordinates).isWalkable 
    //     && !pathFinder.WillBlockPath(coordinates))
    //     {
    //         // Debug.Log($"Creation Called - TowerPrefab:{TowerPrefab.name} - TowerPosition:{transform.position}");
    //         bool isSuccessfull = TowerPrefab.CreateTower(TowerPrefab,transform.position);
    //         if(isSuccessfull){
    //             gridManager.BlockNode(coordinates);
    //             pathFinder.NotifyReceivers();
    //         }
            
    //     }
    // }

    void SpawnObject(GameObject PrefabToSpawn, Vector3 position){
        Instantiate(PrefabToSpawn,position, PrefabToSpawn.transform.rotation);

    }

    bool RandomSpawn(){

        int num = UnityEngine.Random.Range(1,9);
        // Debug.Log("Random value between 1 and 25");

        switch (num)
        {
            case 1:
                return true;
            default:
                return false;
        }
    }
        
    private int GetRandomWeightedIndex(float[] weights)
    {
        float sum = 0f;
        for (int i = 0; i < weights.Length; i++)
        {
            sum = weights[i];
        }

        float randomValue = UnityEngine.Random.Range(0, sum);
        float tempSum = 0;
        for (int i = 0; i < weights.Length; i++)
        {
            // Prefab selection between ranges
            // 0 -> wight[0], weight[0] -> weight[1]
            if(randomValue >=  tempSum && randomValue < tempSum + weights[i])
            {
                return i;
            }
            tempSum += weights[i];
        }
        return 0;
    }
}

// Random place a tree depending on weight
[Serializable]

public struct TreePrefabWeighted
{
    public GameObject prefab;

    [Tooltip("For determining which prefabs will be selected more frequently by random selection")]
    [Range(0,1)]
    public float weight;

}
