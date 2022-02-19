using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    [SerializeField] int cost = 75;
    public int Cost { get { return cost; } }
    [SerializeField] float buildTimer = 1f;
    Bank bank;

    void Start(){
        StartCoroutine(Build());
    }

    public bool CreateTower(Tower tower, Vector3 position)
    {
        bank = FindObjectOfType<Bank>();

        if(bank == null)
        {
            return false;
        }

        if(bank.CurrentBalance >= cost)
        {
        // Debug.Log("Inside Tower ");

            Instantiate(tower.gameObject,position,Quaternion.identity);
            bank.Withdraw(cost);

            return true;
        }

        return false;
    }

    // Create a build timer for tower
    IEnumerator Build(){

// Set object to false
        foreach (Transform child in this.transform)
        {
            child.gameObject.SetActive(false);
            foreach(Transform grandchild in child){
                grandchild.gameObject.SetActive(false);

            }
        }
// set objects to true over time
        foreach (Transform child in this.transform)
        {
            child.gameObject.SetActive(true);
            yield return new WaitForSeconds(buildTimer);
            foreach(Transform grandchild in child){
                grandchild.gameObject.SetActive(true);

            }
        }
    }

}
