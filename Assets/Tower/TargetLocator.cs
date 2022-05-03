using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetLocator : MonoBehaviour
{
    [SerializeField] Transform weapon;

    [SerializeField] Transform baseWeapon;

    [SerializeField] Transform target;

    [SerializeField] Enemy targetEnemy;
    [SerializeField] float range = 15f;
    [SerializeField] ParticleSystem projectyleParticles;

    [SerializeField] float CrystalDamage = 0.1f;

     public enum myEnum // your custom enumeration
    {
        Crystal, 
        Arrow, 
        Cannon
    };
    public myEnum towerType ; 

    // Update is called once per frame
    private void Update()
    {
        FindNearestTarget();
        AimWeapon();
    }

    void FindNearestTarget()
    {
        Enemy[] enemies = FindObjectsOfType<Enemy>();
        Enemy closestEnemy = null;
        Transform closestTarget = null;
        float maxDistance = Mathf.Infinity;

        foreach (var enemy in enemies)
        {
            float targetDistance = Vector3.Distance(transform.position, enemy.transform.position);

            if(targetDistance < maxDistance)
            {
                closestTarget = enemy.transform;
                maxDistance = targetDistance;
                closestEnemy = enemy;
            }
        }

        target = closestTarget;
        targetEnemy = closestEnemy;
    }

    void AimWeapon(){
        if(baseWeapon != null){
            Vector3 targetPostition = new Vector3( target.position.x, 
                                        baseWeapon.transform.position.y, 
                                        target.position.z ) ;
        baseWeapon.LookAt(targetPostition);

        }



        float targetDistance = Vector3.Distance(transform.position,target.position);
        weapon.LookAt(target);
   

        if(targetDistance < range)
        {
            switch (towerType)
            {
                case myEnum.Crystal:
                AttackCrystalTower(true);
                break;
                case myEnum.Arrow:
                AttackArrowTower(true);
                break;
                case myEnum.Cannon:
                AttackCannonTower(true);
                break;

            }

        }else{
            switch (towerType)
            {
                case myEnum.Crystal:
                AttackCrystalTower(false);
                break;
                case myEnum.Arrow:
                AttackArrowTower(false);
                break;
                case myEnum.Cannon:
                AttackCannonTower(false);
                break;

            }
        }
    }

    void AttackArrowTower(bool isActive)
    {
        var emissionModule = projectyleParticles.emission;
        emissionModule.enabled = isActive;
    }

    void AttackCannonTower(bool isActive)
    {
        var emissionModule = projectyleParticles.emission;
        emissionModule.enabled = isActive;

    }

    void AttackCrystalTower(bool isActive)
    {
        LineRenderer lazer = GetComponent<LineRenderer>();
        if(lazer !=null){
        lazer.enabled = isActive;
        if(isActive){
            StartCoroutine(FireLazer());
        }            
        lazer.SetPosition(0,weapon.transform.position);
        lazer.SetPosition(1,targetEnemy.transform.position);
        }




    }

    IEnumerator FireLazer(){

        targetEnemy.enemyHealth.ProcessCrystalHit(CrystalDamage);
        
        yield return new WaitForEndOfFrame();

    }
}
