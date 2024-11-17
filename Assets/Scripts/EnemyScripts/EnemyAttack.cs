using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public int damage;
    public float bulletSpeed;
    public float bulletSize;
    public float fireDelay;
    public GameObject bullet;


    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Attack());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator Attack()
    {
        // TODO add conditional handling to not fire if the player is obstructed by a wall

        while(true)
        {
            yield return new WaitForSeconds(fireDelay);
            // fire raycast
            //Debug.DrawRay(transform.position, transform.up, Color.green, 2.5f);
            RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.up, 30);
            //Debug.Log("enemy aiming at: " + hit.transform.tag);
            if (hit.transform.CompareTag("Player")) {
                GameObject b = Instantiate(bullet, transform.position + (transform.up * 0.5f), transform.rotation);
                b.GetComponent<BulletScript>().MakeEnemyBullet();
                b.GetComponent<BulletScript>().UpdateValues(damage, bulletSpeed, bulletSize);
            }
        }
    }
}
