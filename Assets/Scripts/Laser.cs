
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{

    [SerializeField]
    protected float _speed = 8;
    [SerializeField]
    bool _isEnemyLaser;
    [SerializeField]
    bool _isBeam;
    // Update is called once per frame
    [SerializeField]
    bool _shouldRotate = false;
    [SerializeField]
    bool _shouldRandomlyMove = false;
    float randomMovement = 0;
    int changeMovement = 1;

    private void Awake()
    {
        randomMovement = Random.Range(-1, 2);

    }
    void Update()
    {
        Movement();

        CheckIfOutOfBounds();
    }



    protected virtual void Movement()
    {
        if (!_isEnemyLaser)
        {
            MoveUp();

        }
        else
        {
            MoveDown();
        }

        if (_shouldRotate)
        {
            transform.Rotate(Vector3.forward*5,Space.Self);
        }

        if (_shouldRandomlyMove)
        {
            MoveRightOrLeft();
        }
    }

    //may move randomly left or right, but sometimes it will still not because Random.Range can be 0
    private void MoveRightOrLeft()
    {

        if (randomMovement > 1)
        {
            changeMovement = 2;
        }
        else if(randomMovement<-1)
        {
            changeMovement = -2;

        }

        randomMovement -= Time.deltaTime *changeMovement;


        transform.Translate(Vector3.right* randomMovement * _speed * Time.deltaTime, Space.World);
    }

    private void MoveUp()
    {
        transform.Translate(Vector3.up * _speed * Time.deltaTime, Space.World);
    }

    private void MoveDown()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime, Space.World);
    }

    protected virtual void CheckIfOutOfBounds()
    {
        if (transform.position.y > 8 || transform.position.y<-8f)
        {
            Destruction();
        }
    }

    private void Destruction()
    {
        if (transform.parent != null && !_isBeam)
        {
            Destroy(transform.parent.gameObject);
        }
        else
        {
            Destroy(this.gameObject);

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
                Destroy(this.gameObject);
            }
        }
    }
}
