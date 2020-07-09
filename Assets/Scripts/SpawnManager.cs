using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Wave
{
    public int enemyCount;
    public float enemySpawnTime;
    public int lifePowerUpCount;
    public float lifePowerUpSpawnTime;
    public int missilePowerUpCount;
    public float missilePowerUpSpawnTime;
    public GameObject[] enemies;
    public GameObject[] powerUps;
    public bool bossWave;

}


public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private int _waveNumber;
    [SerializeField]
    private int _enemyAlive;
    [SerializeField]
    private GameObject _enemyContainer;
    [SerializeField]
    private bool _stopSpawning = false;
    [SerializeField]
    private UI_Manager _uiManager;
    public GameObject lifePowerUp;
    public GameObject missilePowerUp;
    [SerializeField]
    public Wave[] waves;

    // Start is called before the first frame update

    void Start()
    {
        _uiManager = GameObject.Find("Canvas").GetComponent<UI_Manager>();

        if (_uiManager == null)
        {
            Debug.LogError("UIMnager is Null!");
        }
        WaveSpawner();
        StartCoroutine(SpawnPowerUpRoutine());
    }

    private void WaveSpawner()
    {
        if (_stopSpawning == false)
        {
            _waveNumber++;
            StartCoroutine(SpawnEnemyRoutine());
            StartCoroutine(SpawnLifePowerUpRoutine());
            StartCoroutine(SpawnMissilePowerUpRoutine());
        }
    }


    IEnumerator SpawnEnemyRoutine()
    {
        if (_waveNumber <= waves.Length)
        {
            _uiManager.WaveText(_waveNumber);
            _uiManager.displayWaveNumber = true;
            yield return new WaitForSeconds(2f);
            _uiManager.displayWaveNumber = false;
            yield return new WaitForSeconds(1f);
            for (int i = 0; i < waves[_waveNumber - 1].enemyCount; i++)
            {
                if (_stopSpawning == false && waves[_waveNumber -1].bossWave == false)
                {
                    int randomEnemy = Random.Range(0, waves[_waveNumber - 1].enemies.Length);
                    Vector3 posToSpawn = new Vector3(Random.Range(-8f, 8f), 7, 0);
                    GameObject newEnemy = Instantiate(waves[_waveNumber - 1].enemies[randomEnemy], posToSpawn, Quaternion.identity);
                    newEnemy.transform.parent = _enemyContainer.transform;
                    _enemyAlive++;
                    yield return new WaitForSeconds(waves[_waveNumber - 1].enemySpawnTime);
                }
                else if (_stopSpawning == false && waves[_waveNumber -1].bossWave == true)
                {
                    GameObject newEnemy = Instantiate(waves[_waveNumber - 1].enemies[0]);
                    newEnemy.transform.parent = _enemyContainer.transform;
                    _enemyAlive++;
                    yield return new WaitForSeconds(waves[_waveNumber - 1].enemySpawnTime);
                }
               
            }
            while (_enemyAlive != 0)
            {
                yield return new WaitForSeconds(1f);
            }
            WaveSpawner();
        }
        else
        {
            _uiManager.GameWonSequence();
            _stopSpawning = true;
        }

    }

    IEnumerator SpawnPowerUpRoutine()
    {
        yield return new WaitForSeconds(10.0f);
        while (_stopSpawning == false)
        {
            Vector3 posToSpawn = new Vector3(Random.Range(-8f, 8f), 7, 0);
            int randomPowerUp = Random.Range(0, waves[_waveNumber - 1].powerUps.Length);
            Instantiate(waves[_waveNumber - 1].powerUps[randomPowerUp], posToSpawn, Quaternion.identity);
            yield return new WaitForSeconds(Random.Range(3.0f, 7.0f));
        }
    }

    IEnumerator SpawnLifePowerUpRoutine()
    {
        for (int i = 0; i < waves[_waveNumber - 1].lifePowerUpCount; i++)
        {
            if (_stopSpawning == false)
            {
                yield return new WaitForSeconds(waves[_waveNumber - 1].lifePowerUpSpawnTime);
                Vector3 posToSpawn = new Vector3(Random.Range(-8f, 8f), 7, 0);
                Instantiate(lifePowerUp, posToSpawn, Quaternion.identity);
                yield return new WaitForSeconds(waves[_waveNumber -1].lifePowerUpSpawnTime);
            }
        }
    }

    IEnumerator SpawnMissilePowerUpRoutine()
    {
        for (int i = 0; i < waves[_waveNumber - 1].missilePowerUpCount; i++)
        {
            if (_stopSpawning == false)
            {
                yield return new WaitForSeconds(waves[_waveNumber - 1].missilePowerUpSpawnTime);
                Vector3 posToSpawn = new Vector3(Random.Range(-8f, 8f), 7, 0);
                Instantiate(missilePowerUp, posToSpawn, Quaternion.identity);
                yield return new WaitForSeconds(waves[_waveNumber - 1].missilePowerUpSpawnTime);
            }
        }
    }


    public void OnEnemyDeath()
    {
        _enemyAlive--;
    }

    public void OnPlayerDeath()
    {
        _stopSpawning = true;
    }

}