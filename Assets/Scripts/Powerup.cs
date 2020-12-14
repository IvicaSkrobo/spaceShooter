using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    [SerializeField]
    float _speed = 3;
    [SerializeField]
    float _powerupId = 0f;
    [SerializeField]
    AudioClip _audioClip;


    private void Update()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y < -5f)
        {
            Destroy(this.gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
               Player player = collision.GetComponent<Player>();

            AudioSource.PlayClipAtPoint(_audioClip,this.transform.position);

            if (player != null)
            {
                switch (_powerupId)
                {
                    case 0:
                        player.ActivateTripleShot();
                        break;
                    case 1:
                        //speed
                        player.SpeedBoostActivate();
                        break;
                    case 2:
                        //shield
                        player.ActivateShield();
                        break;
                    case 3:
                        player.ResetAmmo();
                        break;
                    case 4:
                        player.AddLife();
                        break;
                    case 5:
                        player.ActivateHeatSeek();
                        break;
                }

            
            }
            Destroy(this.gameObject);
        }
    }
}
