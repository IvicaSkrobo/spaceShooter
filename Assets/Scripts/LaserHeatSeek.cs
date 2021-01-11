using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserHeatSeek : Laser
{
    [SerializeField]
    float _radiusSearch = 5f;
    [SerializeField]
    float _rotateSpeed = 1f;

    GameObject target;

    int _direction = 1;


    // Update is called once per frame
    protected override void Movement()
    {
        var hits = Physics2D.OverlapCircleAll(this.transform.position, _radiusSearch);

        foreach (var hit in hits)
        {
            if (hit.CompareTag("Enemy")  && target==null)
            {
                target = hit.transform.gameObject;
            }
        }


        if (target != null)
        {
            var enemy = target.GetComponent<Enemy>();

            if (enemy != null && !enemy.IsDestroyed())
            {
                Vector3 newDirection = target.transform.position - this.transform.position;

                transform.up = Vector3.Lerp(transform.up, newDirection, _rotateSpeed * Time.deltaTime);


                transform.position = Vector3.MoveTowards(this.transform.position, target.transform.position, _speed * Time.deltaTime);

            }
            else
            {

                this.transform.rotation = Quaternion.Euler(0, 0, _direction);

                transform.Translate(Vector3.up * _speed * Time.deltaTime);
            }

        }
        else
        {

            this.transform.rotation = Quaternion.Euler(0, 0, _direction);

            transform.Translate(Vector3.up * _speed * Time.deltaTime);
        }

    }




    protected override void CheckIfOutOfBounds()
    {
        base.CheckIfOutOfBounds();
        if (transform.position.x < -13 || transform.position.x > 13)
        {
            Destroy(this.gameObject);
        }
    }

    public void SetDirection(int index, int direction)
    {
        if (index % 2 == 0)
        {
            _direction = direction;
        }
        else
        {
            _direction = -direction;

        }

    }
}
