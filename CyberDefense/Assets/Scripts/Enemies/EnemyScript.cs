using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyScript : MonoBehaviour
{
    public float speed = 10f;

    public float startHealth;
    private float health; 

    public int moneyReward = 50;

    public GameObject deathEffect;

    [Header("Stuff n things")]
    public Image healthBar;

    private Transform target;
    private int wavePointIndex = 0;

    void Start()
    {   //Gets the amount of waypoints\\
        target = WaypointScript.waypoints[0];
        health = startHealth;
    }

    //Take damage function\\
    public void TakeDamage(float amount)
    {
        health -= amount;

        healthBar.fillAmount = health / startHealth;
        if (health <= 0)
        {
            Die();
        }
    }
    //Death and money reward for enemies\\
    void Die()
    {
        PlayerStats.money += moneyReward;
        GameObject effect = (GameObject) Instantiate(deathEffect, transform.position, Quaternion.identity);
        Destroy(effect, 5f);
        WaveSpawner.enemiesAlive--;
        Destroy(gameObject);
    }

    void Update()
    {   //Movement\\
        Vector3 dir = target.position - transform.position;
        transform.Translate(dir.normalized * speed * Time.deltaTime, Space.World);

        if (Vector3.Distance(transform.position, target.position) <= 0.2f)
        {
            GetNextWaypoint();
        }
    }

    void GetNextWaypoint()
    {   //gets the next waypoint in the WaypointIndex\\
        if (wavePointIndex >= WaypointScript.waypoints.Length - 1)
        {
            EndPath();
            return;
        }
        wavePointIndex++;
        target = WaypointScript.waypoints[wavePointIndex];
    }


    public void EndPath()
    {
        PlayerStats.lives = PlayerStats.lives - 5;
        WaveSpawner.enemiesAlive--;
        Destroy(gameObject);
    }
}
