using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetLocator : MonoBehaviour
{
    [SerializeField] Transform weapon;
    [SerializeField] Transform target;
    [SerializeField] float range = 15f;
    [SerializeField] ParticleSystem projectyleParticles;

    // Update is called once per frame
    private void Update()
    {
        FindNearestTarget();
        AimWeapon();
    }

    void FindNearestTarget()
    {
        Enemy[] enemies = FindObjectsOfType<Enemy>();
        Transform closestTarget = null;
        float maxDistance = Mathf.Infinity;

        foreach (var enemy in enemies)
        {
            float targetDistance = Vector3.Distance(transform.position, enemy.transform.position);

            if(targetDistance < maxDistance)
            {
                closestTarget = enemy.transform;
                maxDistance = targetDistance;
            }
        }

        target = closestTarget;
    }

    void AimWeapon(){

        float targetDistance = Vector3.Distance(transform.position,target.position);
        weapon.LookAt(target);

        if(targetDistance < range)
        {
            Attack(true);
        }else{
            Attack(false);
        }
    }

    void Attack(bool isActive)
    {
        var emissionModule = projectyleParticles.emission;
        emissionModule.enabled = isActive;
    }
}
