using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    private string enemyCode; // unique enemy identifier (concat type, room, enemy count)
    public float maxHealth; // default starting health
    public float speed;
    public Rigidbody2D rb;
    private float curHealth; // remaining health of the enemy
    private float xPOS; // x position on room exit
    private float yPOS; // y position on room exit
    private Vector2 currentVelocity;
    private float timeToMove;
    private bool isMoving = false;
    public bool cleanSpawn = false;

    // Start is called before the first frame update
    void Start()
    {
        cleanSpawn = PreventWallSpawn();
        Movement();
    }

    // Update is called once per frame
    void Update()
    {
        if (!cleanSpawn)
        {
            cleanSpawn = PreventWallSpawn();
        }

        if (isMoving)
        {
            //Debug.Log("Enemy is currently moving");
            timeToMove -= 1 * Time.deltaTime;
            if (timeToMove <= 0)
            {
                //Debug.Log("stopping movement due to time limit");
                isMoving = false;
                Movement();
            }
        } else
        {
            if (rb.velocity != new Vector2(0,0))
            {
                //Debug.Log("setting velocity to 0");
                rb.velocity = new Vector2(0, 0);
            }
        }
        
    }

    private void Movement()
    {
        // get random angle for ray direction
        float xDir = Random.Range(-180, 180);
        float yDir = Random.Range(-180, 180);
        Vector3 rayDirection = new Vector3(xDir, yDir, 0);

        // draws an example ray
        //Debug.DrawRay(transform.position, rayDirection, Color.green, 2.5f);

        // returns the distance that the enemy should move
        float distance = FindDistanceToMove(rayDirection);

        //Debug.Log("Moving enemy at angle: " + transform.eulerAngles.z + " a distance of: " + (distance - 2f));
        if (distance > 2)
        {
            // move enemy distance - buffer
            MoveEnemy(transform.eulerAngles.z, distance - 0.5f);
        } else
        {
            // failed to find suitable movement. Resting for 1 second before trying again.
            isMoving = true;
            timeToMove = 1;
            rb.velocity = new Vector2(0, 0);
        }
    }

    private void MoveEnemy(float angle, float distance)
    {
        isMoving = true;
        float speedX = 0;
        float speedY = 0;
        if (angle <= 90) // facing north west (negative x, positive y)
        {
            // convert angle to radians
            angle *= (Mathf.PI / 180);
            //Debug.Log("enemy is facing north west (negative x, positive y)");
            // use trig to calculate respective x and y speeds
            speedX = Mathf.Sin(angle) * speed;
            speedY = Mathf.Cos(angle) * speed;

            // define velocity value using new speeds
            currentVelocity = new Vector2(-speedX, speedY);

            // calculate distance required to move
            timeToMove = distance / speed;
        }
        else if (angle > 90 && angle <= 180) // facing south west (negative x, negative y)
        {
            //Debug.Log("enemy is facing south west (negative x, negative y)");
            angle -= 90;
            angle *= (Mathf.PI / 180);
            speedX = Mathf.Cos(angle) * speed;
            speedY = Mathf.Sin(angle) * speed;

            currentVelocity = new Vector2(-speedX, -speedY);
            timeToMove = distance / speed;
        }
        else if (angle > 180 && angle <= 270) // facing south east (positive x, negative y)
        {
            //Debug.Log("enemy is facing south east (positive x, negative y)");
            angle -= 180;
            angle *= (Mathf.PI / 180);
            speedX = Mathf.Sin(angle) * speed;
            speedY = Mathf.Cos(angle) * speed;

            currentVelocity = new Vector2(speedX, -speedY);
            timeToMove = distance / speed;
        } else // facing north east (positive x, positive y)
        {
            //Debug.Log("enemy is facing north east (positive x, positive y)");
            angle -= 270;
            angle *= (Mathf.PI / 180);
            speedX = Mathf.Cos(angle) * speed;
            speedY = Mathf.Sin(angle) * speed;

            currentVelocity = new Vector2(speedX, speedY);
            timeToMove = distance / speed;
        }
        //Debug.Log("attempting to move at: " + currentVelocity + " for " + timeToMove + " seconds");
        rb.velocity = currentVelocity;
    }

    private float FindDistanceToMove(Vector3 dir)
    {
        float distance = 0;
        int depth = 0;

        // gives while loop a max depth of 50 to prevent infinite loop
        while(distance <= 2 && depth < 200)
        {
            // creates ray cast in given direction
            Debug.DrawRay(transform.position, dir, Color.green, 2.5f);
            RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, 20);
            distance = hit.distance;
            //Debug.Log("distance: " + distance + " at depth: " + depth);
            
            // ensures raycast is at least 2 units long
            if (distance > 2)
            {
                // faces the enemy in the direction of the valid raycast
                transform.up = dir;
                return distance;
            }
            // recalculates angle on failed attempt
            dir = new Vector3(Random.Range(-180, 180), Random.Range(-180, 180), 0);
            depth++;
        }

        Debug.LogError("Suitable distance was not found");
        return 0;

    }

    private bool PreventWallSpawn()
    {
        /*Debug.Log("checking wall spawn. trigger: " + touchingWallTrigger + ". collision: " + touchingWallCollision);
        int depth = 0;
        while (touchingWallCollision && depth < 50)
        {
            transform.position = new Vector3(Random.Range(-7f, 7f), Random.Range(-3f, 3f), transform.position.z);
            Debug.Log("Moving enemy to" + transform.position + "due to wall spawn. Depth: " + depth);
            depth++;
        }

        return true;*/

        Collider2D coll = GetComponent<Collider2D>();
        ContactFilter2D filter = new ContactFilter2D().NoFilter();
        List<Collider2D> results = new List<Collider2D>();
        if (Physics2D.OverlapCollider(coll, filter, results) > 0)
        {
            transform.position = new Vector3(Random.Range(-7f, 7f), Random.Range(-3f, 3f), transform.position.z);
            Debug.Log("Moving " + gameObject.name + " to" + transform.position + " due to wall spawn.");
        } else
        {
            return true;
        }
        return false;
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Debug.Log("Collided with: " + collision.transform.tag);
        if (collision.transform.CompareTag("Wall"))
        {
            timeToMove = 0;
            rb.velocity = new Vector2(0, 0); ;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log("trigger entered with: " + collision.transform.tag);
        if (collision.transform.CompareTag("Wall"))
        {
            timeToMove = 0;
            rb.velocity = new Vector2(0, 0);
        }
    }


    public void TakeDamage(float dmg)
    {
        curHealth -= dmg;
        // TODO: add death handling
        if (curHealth <= 0)
        {
            EnemyKilled();
        }
    }

    public void SetCurrentHealth(float h)
    {
        curHealth = h;
    }

    public float GetCurHealth()
    {
        return curHealth;
    }

    public Vector2 GetPOS()
    {
        return new Vector2(transform.position.x, transform.position.y);
    }

    private void EnemyKilled()
    {
        GameObject.Find("RoomManager").GetComponent<RoomManagement>().UpdateEnemyOnDeath(gameObject.name);
        Destroy(gameObject);
    }
}
