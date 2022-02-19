using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(Enemy))]
public class EnemyHealth : MonoBehaviour
{
    [SerializeField][Range(0,800)] int maxHitPoints = 5;
    [Tooltip("Add amount to maxHitPoints when enemy dies.")]
    [SerializeField] int difficultyRamp = 1;

    TextMeshPro healthLabel;
    int currentHitPoints = 0;
    Enemy enemy;

    // Start is called before the first frame update
    void OnEnable()
    {
        currentHitPoints = maxHitPoints;
        healthLabel = GetComponentInChildren<TextMeshPro>();
        displayHealth();
    }

    void Start() 
    {
        enemy = GetComponent<Enemy>();
    }

    void OnParticleCollision(GameObject other)
    {
       ProcessHit(); 
    }

    void ProcessHit()
    {
        currentHitPoints--;
        displayHealth();
        // Debug.Log(currentHitPoints);
    
        if(currentHitPoints <= 0)
        {
            gameObject.SetActive(false);
            maxHitPoints += difficultyRamp;
            enemy.RewardGold();
        }
    }

    void displayHealth()
    {
        healthLabel.text = $"{currentHitPoints}";
    }

}
