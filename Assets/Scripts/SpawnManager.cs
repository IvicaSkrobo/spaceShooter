using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [Header("Enemy")]
    [SerializeField] GameObject[] _enemyPrefabs;
    [SerializeField] GameObject _enemyContainer;
    [SerializeField] List<float> _enemyChances;
    [Header("Powerups")]
    [SerializeField] GameObject[] _powerupPrefabs;
    [SerializeField] GameObject _powerupContainer;
    [SerializeField] List<float> _powerupsChances;

    private bool _stopSpawning = false;

    int _waweNumber = 0;
    int _nextWawe = 0;
    int _enemiesPerWawe = 3;

    int _enemiesToSpawn = 3;

    UIManager _uiManager;
    float _sumOfPowerUpChances = 0;
    float _sumOfEnemySpawnChances = 0;

    private void Start()
    {
        _uiManager = FindObjectOfType<UIManager>();

        _powerupsChances = new List<float>();
        _enemyChances = new List<float>();

        for (int i = 0; i<_powerupPrefabs.Length;i++) 
        {
            var chance = _powerupPrefabs[i].GetComponent<Powerup>().GetPowerUpChance();
            _powerupsChances.Add(chance);
            _sumOfPowerUpChances += chance;
        }

       
        for (int i = 0; i < _enemyPrefabs.Length; i++)
        {
            var chance = _enemyPrefabs[i].GetComponent<Enemy>().GetSpawnChance();
            _enemyChances.Add(chance);
            _sumOfEnemySpawnChances += chance;
        }
    }

    public void StartSpawning()
    {
        StartCoroutine(SpawnEnemyRoutine());
        StartCoroutine(SpawnPowerupRoutine());

    }


    public IEnumerator SpawnEnemyRoutine()
    {
        yield return new WaitForSeconds(3f);
        while (!_stopSpawning)
        {

            if (_waweNumber == _nextWawe)
            {
                _nextWawe++;
                _enemiesPerWawe = _waweNumber + 2;
                _enemiesToSpawn = _enemiesPerWawe;

                _uiManager.UpdateWaweText(_waweNumber + 1);

                yield return new WaitForSeconds(5f);

            }

            _enemiesToSpawn--;

            Vector3 posToSpawn = new Vector3(Random.Range(-8f, 8f), 7f, 0);
            var newEnemy = Instantiate(_enemyPrefabs[RandomEnemyChance()], posToSpawn, Quaternion.identity);

            newEnemy.GetComponent<Enemy>().SetSpawnManager(this);

            newEnemy.transform.parent = _enemyContainer.transform;

            yield return new WaitForSeconds(1f);

            if (_enemiesToSpawn == 0)
            {
                yield return new WaitUntil(() => _waweNumber == _nextWawe);
            }
        }
    }

    public void EnemyDestroyed()
    {
        _enemiesPerWawe--;
        if (_enemiesPerWawe == 0)
        {
            _waweNumber++;
        }
    }


    public void OnPlayerDeath()
    {
        _stopSpawning = true;
    }




    public IEnumerator SpawnPowerupRoutine()
    {
        yield return new WaitForSeconds(3f);

        while (!_stopSpawning)

        {
            var randomSpawnTime = Random.Range(3f, 7f);
            yield return new WaitForSeconds(randomSpawnTime);

            Vector3 posToSpawn = new Vector3(Random.Range(-8f, 8f), 7f, 0);

            var randomPowerUp = RandomPowerUpChance(); 


            var newPowerUp = Instantiate(_powerupPrefabs[randomPowerUp], posToSpawn, Quaternion.identity);

            newPowerUp.transform.parent = _powerupContainer.transform;

        }
    }



    public int RandomPowerUpChance()
    {
       var a = Random.Range(0f, _sumOfPowerUpChances);

        float sumOfChances = 0;

        for (int i = 0; i < _powerupsChances.Count; i++)
        {
            sumOfChances += _powerupsChances[i];
            if (a < sumOfChances)
            {
                return i;
            }
        }

        return 0;

    }


    public int RandomEnemyChance()
    {
        var a = Random.Range(0f, _sumOfEnemySpawnChances);

        float sumOfChances = 0;

        for (int i = 0; i < _enemyChances.Count; i++)
        {
            sumOfChances += _enemyChances[i];
            if (a < sumOfChances)
            {
                return i;
            }
        }

        return 0;

    }
}
