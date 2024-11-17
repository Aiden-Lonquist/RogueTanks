using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour
{
    public string doorRoomCode;
    public string doorPOS;
    private GameObject RM;
    private GameObject Player;
    // Start is called before the first frame update
    void Start()
    {
        RM = GameObject.Find("RoomManager");
        Player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetValues(string doorCode, string POS)
    {
        doorRoomCode = doorCode;
        doorPOS = POS;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            RM.GetComponent<RoomManagement>().GoToRoom(doorRoomCode, doorPOS);
            Player.GetComponent<PlayerMovement>().MoveAfterDoor(doorPOS);
        }
    }
}