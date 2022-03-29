using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.UI;
public class DragHandler : MonoBehaviour, IBeginDragHandler,IDragHandler,IEndDragHandler
{
    [SerializeField] Tower prefab;

    // [SerializeField] Image image;


    Tower prefabInstance;

    TextMeshProUGUI towerAmountUI;
    Image image;


    GridManager gridManager;
    Vector2Int coordinates;
    Vector3 tilePosition;

     Pathfinder pathFinder;
     Bank bank;
     bool hasFunds = false;

    // Start is called before the first frame update
    void Start()
    {
        gridManager = FindObjectOfType<GridManager>();
        pathFinder = FindObjectOfType<Pathfinder>();
        bank = FindObjectOfType<Bank>();
        SetUICost();

        prefabInstance = Instantiate(prefab);
        TurnOffTurretShooting();
        AdjustAlpha();
        prefabInstance.gameObject.SetActive(false);
    }

    void SetUICost(){
        towerAmountUI = GetComponentInChildren<TextMeshProUGUI>();
        towerAmountUI.text = prefab.Cost.ToString();
    }

    void TurnOffTurretShooting(){
        Component[] scripts = prefabInstance.GetComponentsInChildren<TargetLocator>();

        foreach (Component script in scripts)
        {
           Destroy(script);
        }

        ParticleSystem[] particleSystems = prefabInstance.GetComponentsInChildren<ParticleSystem>();
        foreach (ParticleSystem particleSystem in particleSystems)
        {
            Destroy(particleSystem);
        }
    }

    void AdjustAlpha(){
        MeshRenderer[] meshRenderers = prefabInstance.GetComponentsInChildren<MeshRenderer>();
        Debug.Log("Adjust Alpha of meshes number" + meshRenderers.Length);
        for (int i = 0; i < meshRenderers.Length; i++)
        {
            Material mat = meshRenderers[i].material;
            meshRenderers[i].material.color = new Color(mat.color.r,mat.color.g,mat.color.b,0.5f);
        }
    }

    // Update is called once per frame
    void Update()
    {
       hasFunds = CheckTowerCost(prefab);
    }


    bool CheckTowerCost(Tower tower){
        image = GetComponentInChildren<Image>();
        if(bank.CurrentBalance < tower.Cost){
            image.color = new Color(image.color.r,image.color.g,image.color.b,0.5f);
            // Debug.Log("Disable UI image: " + image.gameObject.name);
            return false;

        }else{
            image.color = new Color(image.color.r,image.color.g,image.color.b,1f);
            // Debug.Log("Enable UIimage: " + image.gameObject.name);
            return true;
        }
    }

    void IBeginDragHandler.OnBeginDrag(PointerEventData eventData){
        Debug.Log("Begin Drag");
    }

    public void OnDrag(PointerEventData eventData){
        // Debug.Log("Event data while drag " + eventData);
        if(hasFunds){
            RaycastHit[] hits;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            hits = Physics.RaycastAll(ray,1000f);
            if (hits  != null && hits.Length > 0)
            {
                int tileColliderIndex = GetTileColliderIndex(hits);
                if (tileColliderIndex != -1)
                {

                    coordinates = gridManager.GetCoordinatesFromPostion(hits[tileColliderIndex].point);
                    tilePosition = gridManager.GetPositionFromCoordinates(coordinates);
                    prefabInstance.gameObject.SetActive(true);
                    prefabInstance.transform.position = hits[tileColliderIndex].point;
                    // Debug.Log("hit point " + coordinates);
                }else{
                    prefabInstance.gameObject.SetActive(false);
                }
                
            }
        }else{
            return;
        }
    }

    int GetTileColliderIndex(RaycastHit[] hits){

        for (int i = 0; i < hits.Length; i++)
        {
            // Debug.Log("Hits");
            if(hits[i].collider.gameObject.tag == "TileGround"){
                return i;
            }
        }
        return -1;
    }

    public void OnEndDrag(PointerEventData eventData){
        // Debug.Log("On Drag End");
        if(prefabInstance.gameObject.activeSelf){
        // Instantiate(prefab, coordinates,Quaternion.identity);
            if(gridManager.GetNode(coordinates).isWalkable 
            && !pathFinder.WillBlockPath(coordinates)
            && coordinates != pathFinder.StartCoordinates
            && coordinates != pathFinder.EndCoordinates)
            {
                // Debug.Log($"Creation Called - TowerPrefab:{TowerPrefab.name} - TowerPosition:{transform.position}");
                bool isSuccessfull = prefab.CreateTower(prefab,tilePosition);
                if(isSuccessfull){
                    Debug.Log("Create Object");
                    gridManager.BlockNode(coordinates);
                    pathFinder.NotifyReceivers();
                }
            }
        }
        prefabInstance.gameObject.SetActive(false);
    }
}
