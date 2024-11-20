using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    //public Rigidbody2D rb;
    public float damage, speed, size;
    private bool isEnemyBullet = false;
    private float lifeTime = 5;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(0, speed * Time.deltaTime, 0);
        HandleLifeTime();
    }

    public void UpdateValues(float dmg, float spd, float sz)
    {
        damage = dmg;
        speed = spd;
        size = sz;
        transform.localScale *= size;
    }

    public void MakeEnemyBullet()
    {
        isEnemyBullet = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Debug.Log("Collided with: " + collision.transform.tag);
        if (collision.transform.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log("Entered trigger of: " + collision.transform.tag);
        if (collision.transform.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }

        // if enemy do damage
        if (!isEnemyBullet)
        {
            if (!collision.transform.CompareTag("Bullet") && !collision.transform.CompareTag("Player"))
            {
                if (collision.transform.CompareTag("Enemy"))
                {
                    // player's bullet hits an enemy
                    collision.transform.GetComponent<EnemyMovement>().TakeDamage(damage);
                    Destroy(gameObject);
                } else if (collision.transform.CompareTag("Obstacle"))
                {
                    collision.transform.GetComponent<ObstacleScript>().TakeDamage(damage);
                    Destroy(gameObject);
                }
            }
        } else
        {
            if (!collision.transform.CompareTag("Bullet") && !collision.transform.CompareTag("Enemy"))
            {
                if (collision.transform.CompareTag("Player"))
                {
                    // enemy's bullet hits player
                    collision.transform.GetComponent<PlayerMovement>().TakeDamage(damage);
                    Destroy(gameObject);
                } else if (collision.transform.CompareTag("Obstacle"))
                {
                    Destroy(gameObject);
                }
            }
        }
    }

    private void HandleLifeTime()
    {
        lifeTime -= 1 * Time.deltaTime;
        if (lifeTime <= 0)
        {
            Destroy(gameObject);
        }
    }

}

