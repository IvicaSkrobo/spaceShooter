using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [SerializeField]
    float _rotationSpeed = 1f;

    [SerializeField]
    GameObject explosion;

    SpawnManager _spawnManager;

    private void Start()
    {
        _spawnManager = GameObject.FindObjectOfType<SpawnManager>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.forward * _rotationSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Laser"))
        {
            Instantiate(explosion, this.transform.position, Quaternion.identity);


            Destroy(collision.gameObject);

            if (_spawnManager != null)
            {
                _spawnManager.StartSpawning();
            }

            Destroy(this.gameObject,0.25f);
        }
    }
}
