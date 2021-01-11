
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

    [Header("Laser attributes")]
    [SerializeField]
    GameObject _laserPrefab;
    [SerializeField]
    bool _isLaserChild = false;

    [SerializeField]
    Vector3 _offsetLaser;

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
        _anim = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();

        CdChange();

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


        if (Time.time > _cdFire && !isDestroyed  && transform.position.y<7)
        {
            CdChange();
            
            if (_isLaserChild)
            {
                Instantiate(_laserPrefab, transform.position + _offsetLaser, Quaternion.identity,this.transform);
            }
            else
            {
                Instantiate(_laserPrefab, transform.position + _offsetLaser, Quaternion.identity);

            }
        }
    }

    private void Movement()
    {
        if (isDestroyed)
        {
            return;
        }

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

    private void CdChange()
    {
        fireRate = Random.Range(3f, 7f);
        _cdFire = Time.time + fireRate;
    }



    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
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


    public bool IsDestroyed()
    {
        return isDestroyed;
    }
}


