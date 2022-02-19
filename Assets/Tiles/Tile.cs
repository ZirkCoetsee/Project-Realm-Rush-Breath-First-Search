using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] Tower TowerPrefab;
    [SerializeField] bool isPlacable;

    [SerializeField] bool hasObject;


    [SerializeField] GameObject PrefabToSpawn;

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
        if(gridManager != null){
            coordinates = gridManager.GetCoordinatesFromPostion(transform.position);
            if(!isPlacable)
            {
                gridManager.BlockNode(coordinates);
            }else
            {
                if(RandomSpawn() 
                && pathFinder.StartCoordinates != coordinates 
                && pathFinder.EndCoordinates != coordinates
                && !hasObject){
                    // Debug.Log("Yes Random Spawn");
                    gridManager.BlockNode(coordinates);
                    SpawnObject(PrefabToSpawn,transform.position);

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
        Instantiate(PrefabToSpawn,position,Quaternion.identity);
    }

    bool RandomSpawn(){

        int num = Random.Range(1,26);
        // Debug.Log("Random value between 1 and 25");

        switch (num)
        {
            case 5:
                return true;
            default:
                return false;
        }
    }
}
