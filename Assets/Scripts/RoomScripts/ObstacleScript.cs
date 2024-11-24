using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleScript : MonoBehaviour
{
    public float maxHealth;
    public Sprite damagedOne, damagedTwo;
    public SpriteRenderer sr;
    private float curHealth;
    private float xPOS, yPOS;
    // Start is called before the first frame update
    void Start()
    {
        TakeDamage(0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetCurrentHealth(float health)
    {
        curHealth = health;
    }

    public float GetCurrentHealth()
    {
        return curHealth;
    }

    public void TakeDamage(float dmg)
    {
        curHealth -= dmg;
        // TODO: add death handling
        if (curHealth < 3)
        {
            sr.sprite = damagedOne;
        } 
        if (curHealth < 2)
        {
            sr.sprite = damagedTwo;
        }
        if (curHealth <= 0)
        {
            Destroy(gameObject);
        }
    }


}
