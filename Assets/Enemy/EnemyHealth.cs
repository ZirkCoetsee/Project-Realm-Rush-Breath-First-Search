using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

[RequireComponent(typeof(Enemy))]
public class EnemyHealth : MonoBehaviour
{
    [SerializeField][Range(0,800)] int maxHitPoints = 5;
    [Tooltip("Add amount to maxHitPoints when enemy dies.")]
    [SerializeField] int difficultyRamp = 1;

    TextMeshPro healthLabel;
    float currentHitPoints = 0f;

    public float CurrentHitPoints { get { return currentHitPoints; } }
    float healthPercent = 0f;
    Enemy enemy;

    [SerializeField] Image HealthBar;

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
        Debug.Log("Tag: " + other.gameObject.tag);
        if(other.gameObject.tag == "CannonParicle"){
        ProcessCononHit(10);
        }else{
        ProcessHit(); 

        }
    }

    void ConvertHealthToPercentage()
    {
        healthPercent = (float) currentHitPoints / (float) maxHitPoints;
    }

    void ProcessHit()
    {
        currentHitPoints--;
        ConvertHealthToPercentage();
        displayHealth();
        // Debug.Log(currentHitPoints);
    
        if(currentHitPoints <= 0)
        {
            gameObject.SetActive(false);
            maxHitPoints += difficultyRamp;
            enemy.RewardGold();
        }
    }

    public void ProcessCononHit(float damage)
    {
        currentHitPoints = currentHitPoints - damage;
        ConvertHealthToPercentage();
        displayHealth();

        if(currentHitPoints <= 0)
        {
            gameObject.SetActive(false);
            maxHitPoints += difficultyRamp;
            enemy.RewardGold();
        }
    }

    public void ProcessCrystalHit(float damage)
    {
        currentHitPoints = currentHitPoints - damage;
        displayHealth();
        // Debug.Log(currentHitPoints);
    
        if(currentHitPoints <= 0 || currentHitPoints < 1)
        {
            gameObject.SetActive(false);
            maxHitPoints += difficultyRamp;
            enemy.RewardGold();
        }
    }

    void displayHealth()
    {
        ConvertHealthToPercentage();

        healthLabel.text = $"{(int)currentHitPoints}";
        HealthBar.fillAmount = healthPercent;
    }

}
