﻿
using System.Collections;
using UnityEngine;


public enum MovementType
{
    SideToSide, Down, Angular
}

public class Enemy : MonoBehaviour
{
    [SerializeField]
    float _speed = 4f;

    private Player _player;

    Animator _anim;
    AudioSource _audioSource;
    [SerializeField]
    float spawnChance = 0.1f;

    [Header("Shield Attribute")]
    [SerializeField]
    bool _hasShield = false;
    GameObject _shield;
    [SerializeField]
    AudioClip _shieldDestroyClip;

    [Header("Laser attributes")]
    [SerializeField]
    GameObject _laserPrefab;
    [SerializeField]
    bool _isLaserChild = false;

    [SerializeField]
    Vector3 _offsetLaser;

    [Header("Aggresion attribute")]
    [SerializeField]
    bool _isAggresive = false;
    [SerializeField]
    float _aggroToPlayer = 5f;

    [Header("ShootBehind")]
    [SerializeField]
    bool _shouldShootBehind = false;


    bool isDestroyed = false;
    [SerializeField]
    float _cdFire;
    float fireRate;
    [SerializeField]
    MovementType movementType;
    int _randomSideMovement = 0;

    float _posX;
    float _posY;

    [SerializeField]
    float _angle = 0f;
    [SerializeField]
    float _angularSpeed = 0.1f;
    [SerializeField]
    float _radius = 0.2f;

    Vector3 _rotCenter;

    SpawnManager _spawnManager;


   
    public float GetSpawnChance()
    {
        return spawnChance;
    }

    public void SetSpawnManager(SpawnManager spawnManager)
    {
        _spawnManager = spawnManager;
    }
    private void Start()
    {
        _player = FindObjectOfType<Player>();
        _anim = GetComponentInChildren<Animator>();
        _audioSource = GetComponent<AudioSource>();

        if (_hasShield)
        {
            GiveShield();
        }
    }

    public void GiveShield()
    {
        _hasShield = true;
        if (_shield == null)
        {
            _shield = GetComponentInChildren<EnemyShield>(true).gameObject;
        }

        _shield.gameObject.SetActive(true);
    }

    private void OnEnable()
    {
        RandomMovementReset();
    }

    private void RandomMovementReset()
    {

        float randomX = Random.Range(-8f, 8f);

        transform.position = new Vector3(randomX, 8, 0);

        _randomSideMovement = Random.Range(0, 2);

        _rotCenter = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Movement();

        if (transform.position.y < -8)
        {
            RandomMovementReset();
        }

        
        if (_laserPrefab!=null && Time.time > _cdFire && !isDestroyed && transform.position.y < 7)
        {

            if (_shouldShootBehind)
            {

                if (transform.position.y < _player.transform.position.y && Mathf.Abs(transform.position.x - _player.transform.position.x) < 0.5f)
                {
                    CdChange();

                    Instantiate(_laserPrefab, transform.position - _offsetLaser, Quaternion.Euler(0f, 180f, 0f), this.transform);

                }
            }
            else
            {
                CdChange();

                if (_isLaserChild)
                {
                    Instantiate(_laserPrefab, transform.position + _offsetLaser, Quaternion.identity, this.transform);
                }
                else
                {
                    Instantiate(_laserPrefab, transform.position + _offsetLaser, Quaternion.identity);

                }
            }


        }
    }

    private void Movement()
    {
        if (isDestroyed)
        {
            return;
        }



        if (_isAggresive && Vector2.Distance(this.transform.position, _player.transform.position) < _aggroToPlayer && transform.position.y >_player.transform.position.y+1)
        {
            transform.position = Vector3.MoveTowards(transform.position, _player.transform.position, Time.deltaTime * _speed);

        }
        else
        {


            switch (movementType)
            {
                case MovementType.Down:
                    transform.Translate(Vector3.down * Time.deltaTime * _speed);


                    break;
                case MovementType.SideToSide:
                    _posX = 1;
                    if (_randomSideMovement == 0)
                    {
                        _posX = 1;
                    }
                    else
                    {
                        _posX = -1;
                    }

                    transform.Translate(new Vector3(_posX, -0.1f, 0) * Time.deltaTime * _speed);



                    if (transform.position.x <= -11.3)
                    {
                        transform.position = new Vector3(11.3f, transform.position.y, transform.position.z);
                    }
                    else if (transform.position.x >= 11.3)
                    {
                        transform.position = new Vector3(-11.3f, transform.position.y, transform.position.z);
                    }
                    break;

                case MovementType.Angular:

                    _angle += _angularSpeed * Time.deltaTime;

                    var offset = new Vector3(Mathf.Sin(_angle), Mathf.Cos(_angle)) * _radius;
                    transform.position = _rotCenter + offset;

                    _rotCenter.y -= 0.003f;


                    break;
            }


        } 
    }

    private void CdChange()
    {
        fireRate = Random.Range(3f, 7f);
        _cdFire = Time.time + fireRate;
    }



    public void OnTriggerEnter2D(Collider2D other)
    {


        if (other.CompareTag("Player"))
        {
            _isAggresive = false;

            if (_hasShield)
            {

                ShieldDestruction();
                return;
            }

            if (isDestroyed)
            {
                return;
            }
            if (_player != null)
            {
                _player.DamagePlayer();
            }
            _anim.SetTrigger("OnEnemyDeath");
            _audioSource.Play();

            isDestroyed = true;

            _spawnManager.EnemyDestroyed();

            Destroy(this.gameObject, 2.5f);
        }

        if (other.CompareTag("Laser"))
        {
            if (_hasShield)
            {
                ShieldDestruction();

                return;
            }

            if (isDestroyed)
            {
                return;
            }
            Destroy(other.gameObject);
            _speed = 0;

            if (_player != null)
            {
                _player.AddScore(10);
            }
            _audioSource.Play();

            _anim.SetTrigger("OnEnemyDeath");

            isDestroyed = true;
            _spawnManager.EnemyDestroyed();

            Destroy(GetComponent<Collider2D>());
            Destroy(this.gameObject, 2.5f);
        }
    }

    private void ShieldDestruction()
    {
        //TODO remove shield
        _hasShield = false;
        _shield.gameObject.SetActive(false);
        _audioSource.PlayOneShot(_shieldDestroyClip);
        return;
    }

    public bool IsDestroyed()
    {
        return isDestroyed;
    }
}


