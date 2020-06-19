using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Wave
{
    public GameObject[] enemyPrefab;
    public float spawnInterval = 2;
    public int maxEnemies = 20;
}

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private int _waveNumber;
    [SerializeField]
    private int _enemyCount;
    [SerializeField]
    private int _enemyAlive;
    [SerializeField]
    private GameObject[] _enemyPrefab;
//    [SerializeField]
//    private GameObject _enemyFastPrefab;
    [SerializeField]
    private GameObject _enemyContainer;
    [SerializeField]
    private float _spawnTime = 3.0f;
    [SerializeField]
    private bool _stopSpawning = false;
    [SerializeField]
    private GameObject[] _powerUps;
    private UI_Manager _uiManager;
    private Enemy _enemy;
    private GameManager _gameManager;

    public Wave[] waves;
    public int timeBetweenWaves = 5;



    // Start is called before the first frame update

    void Start()
    {
        _uiManager = GameObject.Find("Canvas").GetComponent<UI_Manager>();
        WaveSpawner();
        StartCoroutine(SpawnPowerUpRoutine());
    }


    private void WaveSpawner()
    {
        if (_stopSpawning == false)
        {
            _waveNumber++;
            _enemyCount += 5;
            _spawnTime -= 0.2f;
            StartCoroutine(SpawnEnemyRoutine());
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
            for (int i = 0; i < _enemyCount; i++)
            {
                if (_stopSpawning == false)
                {
                    int randomEnemy = Random.Range(0, 2);
                    Vector3 posToSpawn = new Vector3(Random.Range(-8f, 8f), 7, 0);
                    GameObject newEnemy = Instantiate(_enemyPrefab[randomEnemy], posToSpawn, Quaternion.identity);
                    newEnemy.transform.parent = _enemyContainer.transform;
                    _enemyAlive++;
                    yield return new WaitForSeconds(_spawnTime);
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
        }

    }

    IEnumerator SpawnPowerUpRoutine()
    {
        yield return new WaitForSeconds(10.0f);
        while (_stopSpawning == false)
        {
            Vector3 posToSpawn = new Vector3(Random.Range(-8f, 8f), 7, 0);
            int randomPowerUp = Random.Range(0, 7);
            Instantiate(_powerUps[randomPowerUp], posToSpawn, Quaternion.identity);
            yield return new WaitForSeconds(Random.Range(3.0f, 7.0f));
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