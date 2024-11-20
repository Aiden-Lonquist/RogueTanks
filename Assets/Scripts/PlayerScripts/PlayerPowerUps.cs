using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPowerUps : MonoBehaviour
{
    public GameObject gun;
    public SpriteRenderer treadImage, bodyImage, gunImage;

    public GameObject currentTread, currentBody, currentGun;

    private bool touchingPowerup;
    private GameObject currentPowerUpTouching;
    public List<GameObject> powerUps;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && touchingPowerup)
        {
            string powerUpType = currentPowerUpTouching.GetComponent<PowerUp>().type;
            if (powerUpType == "tread")
            {
                applyTreadPowerUp(currentPowerUpTouching.GetComponent<PowerUp>().speed, currentPowerUpTouching.GetComponent<PowerUp>().powerUpImage);
                if (currentTread != null)
                {
                    Instantiate(currentTread, gameObject.transform.position, gameObject.transform.rotation);
                }
                currentTread = powerUps[currentPowerUpTouching.GetComponent<PowerUp>().index];
            }
            else if (powerUpType == "body")
            {
                applyBodyPowerUp(currentPowerUpTouching.GetComponent<PowerUp>().health, currentPowerUpTouching.GetComponent<PowerUp>().powerUpImage);
                if (currentBody != null)
                {
                    Instantiate(currentBody, gameObject.transform.position, gameObject.transform.rotation);
                }
                currentBody = powerUps[currentPowerUpTouching.GetComponent<PowerUp>().index];
            }
            else if (powerUpType == "gun")
            {
                applyGunPowerUp(currentPowerUpTouching.GetComponent<PowerUp>().damage, currentPowerUpTouching.GetComponent<PowerUp>().fireRate, currentPowerUpTouching.GetComponent<PowerUp>().bulletSpeed, currentPowerUpTouching.GetComponent<PowerUp>().powerUpImage);
                if (currentGun != null)
                {
                    Instantiate(currentGun, gameObject.transform.position, gameObject.transform.rotation);
                }
                currentGun = powerUps[currentPowerUpTouching.GetComponent<PowerUp>().index];
            }
            Destroy(currentPowerUpTouching.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PowerUp"))
        {
            //Debug.Log("touching powerup");
            touchingPowerup = true;
            currentPowerUpTouching = collision.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("PowerUp"))
        {
            //Debug.Log("touching powerup");
            touchingPowerup = false;
            currentPowerUpTouching = null;
        }
    }

    public void applyTreadPowerUp(float speed, Sprite s)
    {
        treadImage.sprite = s;

        if (speed > 0)
            gameObject.GetComponent<PlayerMovement>().speed = speed;
    }

    public void applyBodyPowerUp(float health, Sprite s)
    {
        bodyImage.sprite = s;

        if (health > 0)
            gameObject.GetComponent<PlayerMovement>().health = health;
    }

    public void applyGunPowerUp(float damage, float fireRate, float bulletSpeed, Sprite s)
    {
        gunImage.sprite = s;

        if (damage > 0)
            gun.GetComponent<PlayerAttack>().damage = damage;

        if (fireRate > 0)
            gun.GetComponent<PlayerAttack>().fireRate = fireRate;

        if (bulletSpeed > 0)
            gun.GetComponent<PlayerAttack>().projectileSpeed = bulletSpeed;
    }


    // make a prefab for each power up and put them all in a list.
    // on enemy death there will be a set chance to instantiate one of the powerups in the list
    // so each enemy will need a reference to the list of powerups in the enemy script. 
    // each power up will have it's own script that contains the type and values so that when they are collected the game knows what to do
    // when a power up is collected the stats will be added to the player's stats and the power up will be destroyed.
}
