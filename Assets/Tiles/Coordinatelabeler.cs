using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[ExecuteAlways]
[RequireComponent(typeof(TextMeshPro))]
public class Coordinatelabeler : MonoBehaviour
{
    [SerializeField] Color defaultColor = Color.white;
    [SerializeField] Color blockedColor = Color.gray;
    [SerializeField] Color exploredColor = Color.yellow;
    [SerializeField] Color pathColor = new Color(1F,0.5F,0F);
    TextMeshPro label;
    Vector2Int coordinates = new Vector2Int();
    GridManager gridManager;

    private void Awake() 
    {
        gridManager = FindObjectOfType<GridManager>();
        label = GetComponent<TextMeshPro>();
        label.enabled = false;

        DisplayCoordinates();
    }

    // Update is called once per frame
    void Update()
    {
        if(!Application.isPlaying)
        {
                //Do stuff
                DisplayCoordinates();
                UpdateObjectName();
                //Added for debugging
                // label.enabled = true;
        }
        SetLabelColor();
        ToggleLabels();
    }

    void ToggleLabels()
    {
        if(Input.GetKeyDown( KeyCode.C))
        {
            // label.enabled = !label.IsActive();
            if(label.enabled){
                label.enabled = false;
            }else{
                label.enabled = true;
            }
            
        }
    }
    
    void SetLabelColor()
    {
        if(gridManager == null){
            // Debug.Log($"Node at gridmanager is null");

            return ;
        }
        Node node = gridManager.GetNode(coordinates);

        if(node == null){
            // Debug.Log($"Node at coorditantes:{coordinates} is null");

            return;
        }

            if(!node.isWalkable){
                // Debug.Log("Node is walkable");
                label.color = blockedColor;
                label.enabled = false;
            }else if(node.isPath){
                // Debug.Log("Node is path");
                label.color = pathColor;
                label.enabled = true;

            }else if(node.isExplored){
                // Debug.Log("Node is explored");
                label.color = exploredColor;
                label.enabled = false;
            }else{
                // Debug.Log("Node is default");
                label.color = defaultColor;
                label.enabled = false;
            }


    }

    void DisplayCoordinates()
    {
        if(gridManager == null){
            return;
        }
        coordinates.x = Mathf.RoundToInt(transform.parent.position.x / gridManager.UnityGridSize);
        coordinates.y = Mathf.RoundToInt(transform.parent.position.z / gridManager.UnityGridSize);
        label.text = $"{coordinates.x},{coordinates.y}";
    }

    void UpdateObjectName()
    {
        transform.parent.name = coordinates.ToString();
    }
}
