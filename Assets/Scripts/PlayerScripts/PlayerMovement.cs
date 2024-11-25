using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody2D rb;
    public float speed;
    public float health;

    public GameObject tankTrack;
    private GameObject tracks;
    private bool dashActive = false;
    private bool shieldActive = false;
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        tracks = GameObject.Find("tracks");
        StartCoroutine(PlaceTankTracks());
    }

    // Update is called once per frame
    void Update()
    {
        MovementHandling();
        AdjustRotation();

        if (Input.GetKeyDown(KeyCode.LeftShift) && dashActive)
        {
            rb.velocity *= 5;
        }
    }

    private void MovementHandling()
    {
        float speedX = Input.GetAxisRaw("Horizontal");
        float speedY = Input.GetAxisRaw("Vertical");
        Vector2 playerVelocity = new Vector2(speedX, speedY);
        playerVelocity.Normalize();

        rb.velocity = playerVelocity * speed;
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
                gameObject.transform.position = new Vector3(0, -4f, -2);
                break;
            case "east":
                gameObject.transform.position = new Vector3(-8f, 0, -2);
                break;
            case "south":
                gameObject.transform.position = new Vector3(0, 4f, -2);
                break;
            case "west":
                gameObject.transform.position = new Vector3(8f, 0, -2);
                break;
            default:
                Debug.LogError("DEFAULT REACHED WHEN MOVING PLAYER AFTER DOOR");
                break;
        }
    }

    public bool GetIsMoving()
    {
        if (rb.velocity.x == 0 && rb.velocity.y == 0)
        {
            return false;
        }
        return true;
    }

    public void TakeDamage(float dmg)
    {
        health -= dmg;
        // TODO: add death handling
    }

    private IEnumerator PlaceTankTracks()
    {
        while (true)
        {
            if (rb.velocity.x != 0 || rb.velocity.y != 0)
            {
                Instantiate(tankTrack, gameObject.transform.position + new Vector3(0, 0, 3), gameObject.transform.rotation, tracks.transform);
            }
            yield return new WaitForSeconds(0.3f / speed);

        }
    }

    public void SetDashActive(bool d)
    {
        dashActive = d;
    }
}
