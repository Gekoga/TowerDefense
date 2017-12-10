using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerScript : MonoBehaviour {
    [Header("Attributes")]
    public float range = 15f;

    [Header("Bullet (default)")]
    public float fireRate = 1f;
    private float fireCountdown = 0f;
    public GameObject bulletPrefab;

    [Header("Laser")]
    public bool useLaser = false;
    public int damageOverTime = 30;
    public LineRenderer linerenderer;
    public ParticleSystem impactEffect;
    public float offset = 0.5f;

    [Header("Unity Setup Fields")]
    public float turnspeed = 10f;
    public Transform partToRotate;
    public Transform firePoint;

    [Header("Enemy Information")]
    string enemyTag = "Enemy";
    private Transform target;
    private EnemyScript Enemy;

    // Use this for initialization
    void Start () {
        //activate the UpdateTarget Script
        InvokeRepeating("UpdateTarget", 0f, 0.5f);
    }

    //find enemy
    void UpdateTarget()
    {
        //Put the enemies in a array
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
        //start valuables
        float shortestDistance = Mathf.Infinity;
        GameObject nearestEnemy = null;

        //set enemy as target
        foreach (GameObject enemy in enemies)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            if (distanceToEnemy < shortestDistance)
            {
                shortestDistance = distanceToEnemy;
                nearestEnemy = enemy;
            }
        }
        if (nearestEnemy != null && shortestDistance <= range)
        {
            target = nearestEnemy.transform;
            Enemy = nearestEnemy.GetComponent<EnemyScript>();
        }
        else
        {
            target = null;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //no target? do nothing
        if (target == null)
        {
            if (useLaser)
            {
                if (linerenderer.enabled)
                {
                    linerenderer.enabled = false;
                    impactEffect.Stop();
                }
            }
            return;
        }


        LockOnTarget();

        //is laser? do laser script
        if (useLaser)
        {
            Laser();
        }

        //the shooting rate
        if (fireCountdown <= 0)
        {
            if (!useLaser)
            {
                Shoot();
                fireCountdown = 1f / fireRate;
            }

        }
        fireCountdown -= Time.deltaTime;
    }

    //target lock on
    void LockOnTarget()
    {
        Vector3 dir = target.position - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(dir);
        Vector3 rotation = Quaternion.Lerp(partToRotate.rotation, lookRotation, Time.deltaTime * turnspeed).eulerAngles;
        partToRotate.rotation = Quaternion.Euler(0f, rotation.y, 0f);
    }

    //show the towerrange
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }

    //Shoot the bullet from firepoint
    void Shoot()
    {
        GameObject bulletGO = (GameObject)Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Bullet bullet = bulletGO.GetComponent<Bullet>();

        if (bullet != null)
        {
            bullet.Seek(target);
        }
    }

    //if it is the laser
    void Laser()
    {
        Enemy.TakeDamage(damageOverTime * Time.deltaTime);

        if (!linerenderer.enabled)
        {
            linerenderer.enabled = true;
            impactEffect.Play();
        }

        linerenderer.SetPosition(0, firePoint.position);
        linerenderer.SetPosition(1, target.position);

        Vector3 dir = firePoint.position - target.position;

        impactEffect.transform.position = target.position + dir.normalized * offset;
        impactEffect.transform.rotation = Quaternion.LookRotation(dir);
    }
}
