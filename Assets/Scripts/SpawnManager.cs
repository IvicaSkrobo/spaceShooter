using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
     [Header("Enemy")]
    [SerializeField] GameObject _enemyPrefab;
    [SerializeField] GameObject _enemyContainer;

    [Header("Powerups")]
    [SerializeField] GameObject[] _powerupPrefab;
    [SerializeField] GameObject _powerupContainer;

    private bool stopSpawning = false;

    public void StartSpawning()
    {
        StartCoroutine(SpawnEnemyRoutine());
        StartCoroutine(SpawnPowerupRoutine());

    }


    public IEnumerator SpawnEnemyRoutine()
    {
        yield return new WaitForSeconds(3f);
        while (!stopSpawning)
        {
            Vector3 posToSpawn = new Vector3(Random.Range(-8f, 8f), 7f, 0);
            var newEnemy = Instantiate(_enemyPrefab, posToSpawn, Quaternion.identity);

            newEnemy.transform.parent = _enemyContainer.transform;

            yield return new WaitForSeconds(5f);
        }
    }


    public void OnPlayerDeath()
    {
        stopSpawning = true;
    }




    public IEnumerator SpawnPowerupRoutine()
    {
        yield return new WaitForSeconds(3f);

        while (!stopSpawning)
        
        {
            var randomSpawnTime = Random.Range(3f, 7f);
            yield return new WaitForSeconds(randomSpawnTime);

            Vector3 posToSpawn = new Vector3(Random.Range(-8f, 8f), 7f, 0);
            var randomPowerUp = Random.Range(0, 3);
            var newPowerUp = Instantiate(_powerupPrefab[randomPowerUp], posToSpawn, Quaternion.identity);

            newPowerUp.transform.parent = _powerupContainer.transform;

        }
    }


}
