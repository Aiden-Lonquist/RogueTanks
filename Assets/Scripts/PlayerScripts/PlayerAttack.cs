using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public float damage;
    public float fireRate;
    public float projectileSpeed;
    public float projectileSize;
    public bool fullAuto;
    private float cooldown;
    public GameObject projectile;
    private bool stationaryShooting = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        cooldown -= fireRate * Time.deltaTime;
        if (!fullAuto)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Attack();
            }
        } else
        {
            if (Input.GetMouseButton(0))
            {
                Attack();
            }
        }
    }

    public void NewWeapon(GameObject nw)
    {
        //nw.GetComponent<script>().speed;
        //"" damage
        //"" size
        //"" isFullAuto
        //"" fireRate
    }

    private void Attack()
    {
        if (cooldown <= 0)
        {
            if (stationaryShooting) // check if the player is standing still if they need to
            {
                if (!gameObject.GetComponentInParent<PlayerMovement>().GetIsMoving())
                {
                    cooldown = 1;
                    GameObject shot = Instantiate(projectile, transform.position + (transform.up * 0.5f), transform.rotation);
                    shot.GetComponent<BulletScript>().UpdateValues(damage, projectileSpeed, projectileSize);
                }
            }
            else
            {
                cooldown = 1;
                GameObject shot = Instantiate(projectile, transform.position + (transform.up * 0.5f), transform.rotation);
                shot.GetComponent<BulletScript>().UpdateValues(damage, projectileSpeed, projectileSize);
            }
        }
    }

    public void SetStationairyShooting(bool ss)
    {
        stationaryShooting = ss;
    }
}
