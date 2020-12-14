using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Player attributes")]
    [SerializeField]
    private int _lives = 3;

    [SerializeField]
    private float _speed = 3.5f;

    [Header("SpeedBoost")]
    private float _speedMultiplier = 2f;
    private bool isSpeedBoostActive = false;

    [Header("Laser attributes")]
    [SerializeField]
    GameObject _laserPrefab;



    [SerializeField]
    Vector3 _offsetLaser;

    [SerializeField]
    float _fireRate = 0.5f;

    float _canFire = -1f;

    [Header("TripleShot")]
    [SerializeField]
    GameObject _tripleShot;
    private bool isTripleShotActive = false;


    [Header("Shield")]
    [SerializeField] GameObject shield;
    private bool isShieldActive = false;

    [Header("EngineDamage")]
    [SerializeField]
    GameObject[] _engineDamage;

    SpawnManager _spawnManager;
    UIManager _uIManager;
    int _score = 0;

    // Start is called before the first frame update
    void Start()
    {
        this.transform.position = Vector3.zero;
        _uIManager = FindObjectOfType<UIManager>();
        _spawnManager = FindObjectOfType<SpawnManager>();
    }


    // Update is called once per frame
    void Update()
    {
        CalculateMovement();

        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)
        {
            FireLaser();
        }
    }



    private void FireLaser()
    {
        if (isTripleShotActive)
        {
            Instantiate(_tripleShot, transform.position, Quaternion.identity);

        }
        else
        {
            Instantiate(_laserPrefab, transform.position + _offsetLaser, Quaternion.identity);

        }
        _canFire = Time.time + _fireRate;
    }

    void CalculateMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);

        if (isSpeedBoostActive)
        {
            transform.Translate(direction * _speed *_speedMultiplier* Time.deltaTime);
        }
        else
        {
            transform.Translate(direction * _speed * Time.deltaTime);

        }




        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -3.8f, 0), 0);

        if (transform.position.x <= -11.3)
        {
            transform.position = new Vector3(11.3f, transform.position.y, transform.position.z);
        }
        else if (transform.position.x >= 11.3)
        {
            transform.position = new Vector3(-11.3f, transform.position.y, transform.position.z);
        }
    }

    public void DamagePlayer()
    {
        if (isShieldActive)
        {
            shield.gameObject.SetActive(false);
            isShieldActive = false;
            return;
        }

        _lives--;


        _uIManager.UpdateLives(_lives);
        if (_lives <= 0)
        {
            Destroy(this.gameObject);
            return;
        }


        DamageTheEngine();

    }

    private void DamageTheEngine()
    {
        var randEngine = UnityEngine.Random.Range(0, 2);
        while (_engineDamage[randEngine].gameObject.activeInHierarchy)
        {
            randEngine = UnityEngine.Random.Range(0, 2);
        }
        _engineDamage[randEngine].gameObject.SetActive(true);

    }

    public void ActivateTripleShot()
    {
        isTripleShotActive = true;
        StartCoroutine(TripleShootPowerDown());
    }

    private IEnumerator TripleShootPowerDown()
    {
        yield return new WaitForSeconds(5f);
        isTripleShotActive = false;
    }

    public void SpeedBoostActivate()
    {
        isSpeedBoostActive = true;
        StartCoroutine(SpeedBoostPowerDown());
    }

    private IEnumerator SpeedBoostPowerDown()
    {
        yield return new WaitForSeconds(5f);
        isSpeedBoostActive = false;

    }

    public void ActivateShield()
    {
        isShieldActive = true;
        shield.gameObject.SetActive(true);
        StartCoroutine(ShieldPowerDown());

    }

    private IEnumerator ShieldPowerDown()
    {
        yield return new WaitForSeconds(5f);
        isShieldActive = false;
        shield.gameObject.SetActive(false);
    }

    public void AddScore(int points)
    {
        _score += points;
        _uIManager.UpdateScore(_score);
    }

}
