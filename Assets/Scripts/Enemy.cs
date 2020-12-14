
using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    float _speed = 4f;

    private Player _player;

    Animator _anim;
    [SerializeField]
    AudioSource _audioSource;

    [Header("Laser attributes")]
    [SerializeField]
    GameObject _laserPrefab;
    [SerializeField]
    Vector3 _offsetLaser;

    bool isDestroyed = false;
    [SerializeField]
    float  _cdFire;
    float fireRate;

    private void Start()
    {
        _player = FindObjectOfType<Player>();
        _anim = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();

        CdChange();

    }
    // Update is called once per frame
    void Update()
    {


          transform.Translate(Vector3.down * Time.deltaTime * _speed);

        if (transform.position.y < -8)
        {
            float randomX = Random.Range(-8f, 8f);

            transform.position = new Vector3(randomX, 7, 0);
        }


        if (Time.time > _cdFire && !isDestroyed)
        {
            CdChange();
            Instantiate(_laserPrefab, transform.position + _offsetLaser, Quaternion.identity);
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

            if (_player != null)
            {
                _player.DamagePlayer();
            }
            _anim.SetTrigger("OnEnemyDeath");
            _audioSource.Play();

            isDestroyed = true;

            Destroy(this.gameObject, 2.5f);
        }

        if (other.CompareTag("Laser"))
        {
            Destroy(other.gameObject);
            _speed = 0;

            if (_player!= null)
            {
                _player.AddScore(10);
            }
            _audioSource.Play();

            _anim.SetTrigger("OnEnemyDeath");

            isDestroyed = true;

            Destroy(GetComponent<Collider2D>());
            Destroy(this.gameObject, 2.5f);
        }
    }
}


