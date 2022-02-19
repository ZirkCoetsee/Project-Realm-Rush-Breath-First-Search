using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [SerializeField] Vector2Int gridSize;

    [Tooltip("World Grid Size - should match the unity editor snap settings.")]
    [SerializeField] int unityGridSize = 10;
    public int UnityGridSize { get { return unityGridSize; } }

    Dictionary<Vector2Int,Node> grid = new Dictionary<Vector2Int, Node>();
    public Dictionary<Vector2Int,Node> Grid { get { return grid;}}
    private void Awake() {
        createGrid();
    }

    public Node GetNode(Vector2Int coordinates){
        
        if(grid.ContainsKey(coordinates))
        {
            return grid[coordinates];
        }

        return null;
    }

    public void BlockNode(Vector2Int coordinates){
        if(grid.ContainsKey(coordinates))
        {
            grid[coordinates].isWalkable = false;
        }
    }

    //Reset the node info for when we create a new path while game is running 
    public void ResetNodes(){

        foreach (KeyValuePair<Vector2Int,Node> entry in grid)
        {
            // Reset conneceted to
            entry.Value.connectedTo = null;
            entry.Value.isExplored = false;
            entry.Value.isPath = false;
        }
    }

    public Vector2Int GetCoordinatesFromPostion(Vector3 position){

        Vector2Int coordinates = new Vector2Int();
                
        // Taking the position of the tiles and deviding it by the 3D world girdsize to get the snap tile positions
        coordinates.x = Mathf.RoundToInt(position.x /unityGridSize);
        coordinates.y = Mathf.RoundToInt(position.z /unityGridSize);

        return coordinates;
    }

    public Vector3 GetPositionFromCoordinates(Vector2Int coordinates){
        
        Vector3 position = new Vector3();
        position.x = coordinates.x * unityGridSize;
        position.z = coordinates.y * unityGridSize;

        return position;

    }

    void createGrid(){

        //Loop through x components
        for( int x = 0; x < gridSize.x; x++){
            // Loop through y components
            for (int y = 0; y < gridSize.y; y++)
            {
                Vector2Int coordinates = new Vector2Int(x,y);
                grid.Add(coordinates,new Node(coordinates,true));
                // Debug.Log(grid[coordinates].coordinates + " = " + grid[coordinates].isWalkable);
            }
        }
    }
}
