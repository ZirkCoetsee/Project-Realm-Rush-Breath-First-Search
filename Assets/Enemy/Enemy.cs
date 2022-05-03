using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] int goldReward = 25;
    [SerializeField] int goldPenalty = 1;
    Bank bank;

    public EnemyHealth enemyHealth;
    // Start is called before the first frame update
    void Start()
    {
        bank = FindObjectOfType<Bank>();
        enemyHealth = GetComponent<EnemyHealth>();
    }

    public void RewardGold()
    {
        if(bank != null)
        {
            bank.Deposit(goldReward);
        }
            else
        {
            return;
        }
    }

    public void SteelGold()
    {
        if(bank != null)
        {
            bank.Withdraw(goldPenalty * (int)enemyHealth.CurrentHitPoints);
        }
            else
        {
            return;
        }
    }
    private void OnMouseDown() {
        CameraMovement.instance.followTransform = transform;
    }
}
