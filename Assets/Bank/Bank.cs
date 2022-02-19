using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Bank : MonoBehaviour
{
    [SerializeField] int startingBalance = 150;
    [SerializeField] int currentBalance;
    [SerializeField] TextMeshProUGUI displayBalance;
    [SerializeField] TextMeshProUGUI displayKilled;

    int enemyKillCount = 0;


    public int CurrentBalance { get{ return currentBalance; } }

    void Awake() 
    {
        currentBalance = startingBalance;
        UpdateDisplay();
    }

    public void Deposit(int amount)
    {
        currentBalance += Mathf.Abs(amount);
        enemyKillCount++;
        UpdateDisplay();
        UpdateKilledDisplay();
    }

    public void Withdraw(int amount)
    {
        currentBalance -= Mathf.Abs(amount);  
        UpdateDisplay();
        if(currentBalance < 0){
           ReloadScene();
        }      
    }

    void ReloadScene(){
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.buildIndex);
    }

    void UpdateDisplay(){
        displayBalance.text = $"Gold: {currentBalance}";
    }

    void UpdateKilledDisplay(){
        displayKilled.text = $"Enemies Killed: {enemyKillCount}";
    }

}
