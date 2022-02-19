using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinder : MonoBehaviour
{
    [SerializeField] Vector2Int startCoordinates;
    public Vector2Int StartCoordinates {get {return startCoordinates;}}
    [SerializeField] Vector2Int endCoordinates;
    public Vector2Int EndCoordinates {get {return endCoordinates;}}

    Node startNode; 
    Node endNode;
    Node currentSearchNode;

    Queue<Node> frontier = new Queue<Node>();
    Dictionary<Vector2Int,Node> reached = new Dictionary<Vector2Int, Node>();
    
    Vector2Int[] directions = {Vector2Int.right, Vector2Int.left, Vector2Int.up, Vector2Int.down};

    GridManager gridManager;

    Dictionary<Vector2Int,Node> grid = new Dictionary<Vector2Int, Node>();

    private void Awake() {
        gridManager = FindObjectOfType<GridManager>();
        if(gridManager != null){

            grid = gridManager.Grid;
            startNode = grid[startCoordinates];
            endNode = grid[endCoordinates];
        }

    }
    // Start is called before the first frame update
    void Start()
    {
        GetNewPath();
    }

    public List<Node> GetNewPath(){
        return GetNewPath(startCoordinates);
    }

    public List<Node> GetNewPath(Vector2Int coordinates){
        // we call reset nodes on grid manager to clear pervs path's nodes info
        gridManager.ResetNodes();
        BreathFirstSearch(coordinates);
        return BuildPath();
    }

    void ExploreNeighbors(){
        List<Node> neighbors = new List<Node>();

        foreach(Vector2Int direction in directions)
        {

            Vector2Int checkCoordinates = currentSearchNode.coordinates + direction;
        
            if(grid.ContainsKey(checkCoordinates))
            {
                // Was
                // neighbors.Add(new Node(checkCoordinates,true));
                // Should be
                neighbors.Add(grid[checkCoordinates]);
            }
        }

        foreach(Node neighbor in neighbors)
        {
            if(!reached.ContainsKey(neighbor.coordinates) && neighbor.isWalkable)
            {
                neighbor.connectedTo = currentSearchNode;
                reached.Add(neighbor.coordinates,neighbor);
                frontier.Enqueue(neighbor);
            }
        }
    }

    void BreathFirstSearch(Vector2Int coordinates)
    {
        // Exception to the rules set the start and end node to be walkable
        startNode.isWalkable = true;
        endNode.isWalkable = true;

        // For when we search a new path after it has been blocked
        frontier.Clear();
        reached.Clear();

        bool isRunning = true;

        frontier.Enqueue(grid[coordinates]);
        reached.Add(coordinates,grid[coordinates]);

        while(frontier.Count > 0 && isRunning)
        {
            currentSearchNode = frontier.Dequeue();
            currentSearchNode.isExplored = true;
            // Debug.Log(currentSearchNode.coordinates);
            ExploreNeighbors();
            if(currentSearchNode.coordinates == endCoordinates){
                isRunning = false;
            }
        }
    }

    //If there is no path to end node then we can not place a tower there 
    public bool WillBlockPath(Vector2Int coordinates)
    {
        if(grid.ContainsKey(coordinates))
        {
            // Save the initial state
            bool previousState = grid[coordinates].isWalkable;

            // Temp set the value to not walkable to run a breathfirst search on the path with the selected node blocked
            grid[coordinates].isWalkable = false;
            // Save that new path with the blocked node
            List<Node> newPath = GetNewPath();
            // And reset it to previous state as to not mess with the current path
            grid[coordinates].isWalkable = previousState;

            //The path will be blocked because it could not get further than the single node 
            if(newPath.Count <= 1){
                GetNewPath();
                return true;
            }
        }
            return false;
    }

    List<Node> BuildPath(){
        List<Node> path = new List<Node>();
        // Remember we now have all the nodes to the end node
        // So we start at the end of the tree and work backwards
        Node currentNode = endNode;

        //We add our first node - or our last node in the tree the destination  
        path.Add(currentNode);
        currentNode.isPath = true;

        // Now we loop through all the nodes connected to this node
        while (currentNode.connectedTo != null)
        {
            // Set the currentNode to currentNode.connectedTo
            // Takes us one step back down our connected path
            currentNode = currentNode.connectedTo;
            // add currentNode to path
            path.Add(currentNode);
            currentNode.isPath = true;
        }

        // Now we take the reversed path we made and reverse it again to have it now order from the startnode in stead
        path.Reverse();
        return path;
    }

    public void NotifyReceivers(){
        BroadcastMessage("RecalculatePath", false,SendMessageOptions.DontRequireReceiver);
    }
}
