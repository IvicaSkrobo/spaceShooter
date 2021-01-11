using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [Header("Enemy")]
    [SerializeField] GameObject[] _enemyPrefab;
    [SerializeField] GameObject _enemyContainer;

    [Header("Powerups")]
    [SerializeField] GameObject[] _powerupPrefab;
    [SerializeField] GameObject _powerupContainer;

    private bool stopSpawning = false;

    int waweNumber = 0;
    int nextWawe = 0;
    int enemiesPerWawe = 3;

    int enemiesToSpawn = 3;

    UIManager _uiManager;

    private void Start()
    {
        _uiManager = FindObjectOfType<UIManager>();
    }

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
            
            if (waweNumber == nextWawe)
            {
                nextWawe++;
                enemiesPerWawe = waweNumber + 2;
                enemiesToSpawn = enemiesPerWawe;

                _uiManager.UpdateWaweText(waweNumber+1);
                
                yield return new WaitForSeconds(5f);

            }

            enemiesToSpawn--;

            Vector3 posToSpawn = new Vector3(Random.Range(-8f, 8f), 7f, 0);
            var newEnemy = Instantiate(_enemyPrefab[Random.Range(0,_enemyPrefab.Length)], posToSpawn, Quaternion.identity);

            newEnemy.GetComponent<Enemy>().SetSpawnManager(this);

            newEnemy.transform.parent = _enemyContainer.transform;

            yield return new WaitForSeconds(1f);

            if (enemiesToSpawn == 0)
            {
                yield return new WaitUntil(() => waweNumber == nextWawe);
            }
        }
    }

    public void EnemyDestroyed()
    {
        enemiesPerWawe--;
        if (enemiesPerWawe == 0)
        {
            waweNumber++;
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

            var randomPowerUp = Random.Range(0, _powerupPrefab.Length);


            //give it a chance to not spawn the heat seeking power up
            if (randomPowerUp == 5)
            {
               if (Random.Range(0, 4) < 1)
                {
                    randomPowerUp = Random.Range(0, _powerupPrefab.Length);
                }
                
            }

            var newPowerUp = Instantiate(_powerupPrefab[randomPowerUp], posToSpawn, Quaternion.identity);

            newPowerUp.transform.parent = _powerupContainer.transform;

        }
    }


}
