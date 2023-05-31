using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject[] enemyPrefabs;
    public GameObject[] powerupPrefabs;
    public GameManager gameScript;

    private float spawnRange = 9;
    public int enemyCount;
    public int waveNumber = 1;

    // Start is called before the first frame update
    void Start()
    {
        SpawnEnemyWave(waveNumber);
        SpawnPowerups(waveNumber);
    }

    // Update is called once per frame
    void Update()
    {
        enemyCount = GameObject.FindGameObjectsWithTag("Enemy").Length;
        if(enemyCount == 0 && !gameScript.isGameOver)
        {
            waveNumber++;
            Debug.Log("Preparing the wave "+waveNumber);
            if (waveNumber%5==0)
            {
                int bossNumber = (int)waveNumber/5;
                SpawnPowerups(waveNumber);
                SpawnBoss(bossNumber);
            }
            else
            {
                SpawnEnemyWave(waveNumber);
                SpawnPowerups(waveNumber);
            }
        }
        
    }

    private Vector3 generateSpawnPos()
    {
        float spawnPosX = Random.Range(-spawnRange, spawnRange);
        float spawnPosZ = Random.Range(-spawnRange, spawnRange);
        Vector3 randomPos = new Vector3(spawnPosX,0,spawnPosZ);
        return randomPos;
    }

    void SpawnEnemyWave(int enemiesToSpawn) {
        for(int i=0; i< enemiesToSpawn; i++){
            int randomEnemy = Random.Range(0,enemyPrefabs.Length - 1);
            Instantiate(enemyPrefabs[randomEnemy], generateSpawnPos(), enemyPrefabs[randomEnemy].transform.rotation);
        }
    }

    void SpawnPowerups(int powerupsToSpawn){
        GameObject[] powerups = GameObject.FindGameObjectsWithTag("Powerup");

        foreach(GameObject powerup in powerups)
        {
            Destroy(powerup);
        }

        for(int i=0; i< powerupsToSpawn; i++){
            int randomPowerup = Random.Range(0,powerupPrefabs.Length);
            Instantiate(powerupPrefabs[randomPowerup], generateSpawnPos(), powerupPrefabs[randomPowerup].transform.rotation);
        }
    }

    void SpawnBoss(int bossStrength){
        GameObject boss = enemyPrefabs[enemyPrefabs.Length-1]; 
        Instantiate(boss, generateSpawnPos(), boss.transform.rotation);
    }
}
