using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody2D rb;
    public int speed;
    public float health;
    private bool dashActive = false;
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        MovementHandling();
        AdjustRotation();

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            //Dash();
        }
    }

    private void MovementHandling()
    {
        float speedX = Input.GetAxisRaw("Horizontal");
        float speedY = Input.GetAxisRaw("Vertical");
        rb.velocity = new Vector2(speedX, speedY) * speed;
    }

    private void AdjustRotation()
    {
        if (rb.velocity.x != 0 || rb.velocity.y != 0)
        {
            Vector2 v = rb.velocity;
            float angle = Mathf.Atan2(v.y, v.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }
    
    public void MoveAfterDoor(string doorPOS)
    {
        switch(doorPOS)
        {
            case "north":
                gameObject.transform.position = new Vector3(0, -3f, 0);
                break;
            case "east":
                gameObject.transform.position = new Vector3(-7f, 0, 0);
                break;
            case "south":
                gameObject.transform.position = new Vector3(0, 3f, 0);
                break;
            case "west":
                gameObject.transform.position = new Vector3(7f, 0, 0);
                break;
            default:
                Debug.LogError("DEFAULT REACHED WHEN MOVING PLAYER AFTER DOOR");
                break;
        }
    }

    public void TakeDamage(float dmg)
    {
        health -= dmg;
        // TODO: add death handling
    }
}
