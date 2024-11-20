using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleScript : MonoBehaviour
{
    public float maxHealth;
    private float curHealth;
    private float xPOS, yPOS;
    // Start is called before the first frame update
    void Start()
    {

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
        if (curHealth <= 0)
        {
            Destroy(gameObject);
        }
    }


}