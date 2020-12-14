using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{

    [SerializeField]
    float _speed = 8;
    [SerializeField]
    bool _isEnemyLaser;
    // Update is called once per frame
    void Update()
    {
        Movement();

        Destruction();
    }



    private void Movement()
    {
        if (!_isEnemyLaser)
        {
            MoveUp();

        }
        else
        {
            MoveDown();
        }

    }

    private void MoveUp()
    {
        transform.Translate(Vector3.up * _speed * Time.deltaTime);
    }

    private void MoveDown()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);
    }

    private void Destruction()
    {
        if (transform.position.y > 8 || transform.position.y<-8f)
        {
            if (transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }
            else
            {
                Destroy(this.gameObject);

            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && _isEnemyLaser)
        {
            Player _player= collision.GetComponent<Player>();

            if (_player != null)
            {
                _player.DamagePlayer();
            }
        }
    }
}
