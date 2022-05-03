using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
[RequireComponent(typeof(Enemy))]
public class EnemyMover : MonoBehaviour
{
    [SerializeField][Range(0f,5f)] float Speed = 1f;
    List<Node> path = new List<Node>();
    Enemy enemy ;
    GridManager gridManager;
    Pathfinder pathFinder;
    Animator _animator;

    void OnEnable()
    {
        ReturnToStart();
        RecalculatePath(true);
    }

    void Awake()
    {
        enemy = GetComponent<Enemy>();
        gridManager = FindObjectOfType<GridManager>();
        pathFinder = FindObjectOfType<Pathfinder>();
        _animator = GetComponentInChildren<Animator>();
        _animator.SetFloat("Speed", Speed);
    }

    void RecalculatePath(bool resetPath)
    {
        // Debug.Log("Recalculate new path");
        Vector2Int coordinates = new Vector2Int();

        if(resetPath)
        {
            coordinates = pathFinder.StartCoordinates;
        }else
        {
            coordinates = gridManager.GetCoordinatesFromPostion(transform.position);
        }

        StopAllCoroutines();
        path.Clear();
        path = pathFinder.GetNewPath(coordinates);
        StartCoroutine(FollowPath());
    }

    void ReturnToStart()
    {
        // Debug.Log("Return to start");
        transform.position = gridManager.GetPositionFromCoordinates(pathFinder.StartCoordinates);
    }

    void FinishPath(){
        // Debug.Log("Finished Path");
        enemy.SteelGold();
        gameObject.SetActive(false);
    }

    IEnumerator FollowPath(){

        for(int i = 1; i < path.Count; i++)
        {
            // Debug.Log("Path count:" + path.Count);
            // Debug.Log("i count:" + i);

            Vector3 startPostition = transform.position;
            Vector3 endPosition = gridManager.GetPositionFromCoordinates(path[i].coordinates);
            float travelPercent = 0f;
            // Debug.Log($"Start position {startPostition} : End Point = {endPosition}");
            

            transform.LookAt(endPosition);

            while(travelPercent < 1f){
                travelPercent += Time.deltaTime * Speed;
                this.transform.position = Vector3.Lerp(startPostition,endPosition,travelPercent);
                yield return new WaitForEndOfFrame();
            }
        }

        FinishPath();


    }
}
