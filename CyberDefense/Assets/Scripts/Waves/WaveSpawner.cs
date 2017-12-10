using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaveSpawner : MonoBehaviour {

    public static int enemiesAlive = 0;
    public int enemiesAliveDebug = 0;

    [Header("Enemy types")]
    public Wave[] waves;

    [Header("Spawn points")]
    public Transform spawnPoint;


    [Header("Timers")]
    public float countDownTimer = 60;
    public float countDown = 2f;

    [Header("Wave")]
    public int waveIndex = 0;

    [Header("Text")]
    public Text countDownTimerText;

    void Update()
    {
        enemiesAliveDebug = enemiesAlive;
        if (enemiesAlive > 0)
        {
            return;
        }

        if (countDown <= 0f)
        {
            StartCoroutine(SpawnWave());
            countDown = countDownTimer;
            return;
        }
        countDown -= Time.deltaTime;

        countDownTimerText.text = Mathf.Round(countDown).ToString();
    }

    IEnumerator SpawnWave()
    {
        PlayerStats.rounds++;

        Wave wave = waves[waveIndex];

        for (int i = 0; i < wave.count; i++)
        {
            SpawnEnemy(wave.enemy);
            yield return new WaitForSeconds(1f / wave.rate);
        }
        waveIndex++;

        if (waveIndex == waves.Length && enemiesAlive == 0)
        {
            Time.timeScale = 0;
            IGMenu.instance.inGameUI.SetActive(false);
            IGMenu.instance.winMenu.SetActive(true);
            this.enabled = false;
        }
    }

    public void SpawnEnemy(GameObject enemy)
    {
        Instantiate(enemy, spawnPoint.position, spawnPoint.rotation);
        enemiesAlive++;
    }
}
