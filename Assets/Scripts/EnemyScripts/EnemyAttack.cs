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
        // wait a random amount of time before firing first shot to give player the chance to move 
        yield return new WaitForSeconds(Random.Range(0.5f, 2f));

        while (true)
        {
            // fire raycast
            //Debug.DrawRay(transform.position, transform.up, Color.green, 2.5f);
            RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.up, 30);
            //Debug.Log("enemy aiming at: " + hit.transform.tag);
            // if player is in sight then a shot will fire
            if (hit.transform.CompareTag("Player")) {
                GameObject b = Instantiate(bullet, transform.position + (transform.up * 0.5f), transform.rotation);
                b.GetComponent<BulletScript>().MakeEnemyBullet();
                b.GetComponent<BulletScript>().UpdateValues(damage, bulletSpeed, bulletSize);
                yield return new WaitForSeconds(fireDelay);
            } else
            {
                // if fails to fire delay until next attack is shorter
                yield return new WaitForSeconds(fireDelay/3f);
            }
        }
    }
}
