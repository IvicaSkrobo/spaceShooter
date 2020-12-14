using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserSecondary : Laser
{
    [SerializeField] GameObject heatSeekPrefab;
    Vector3 startingPos;
    [SerializeField]
    int amountOfHeatSeekLasers = 4;
    int direction = 15;
    private void OnEnable()
    {
        startingPos = this.transform.position;
    }

    protected override void Movement()
    {
        base.Movement();

        if (this.transform.position.y > (startingPos.y + 3f))
        {


            //instantiate 2 heatseeking lasers and destroy this one
            for (int i = 0; i < amountOfHeatSeekLasers; i++)
            {
                if (i != 0 && i % 2 == 0)
                {
                    direction += 15;
                }
                var heatSeek = Instantiate(heatSeekPrefab, this.transform.position, Quaternion.identity);
                heatSeek.GetComponent<LaserHeatSeek>().SetDirection(i, direction);
            }




            Destroy(this.gameObject);
        }
    }
}
