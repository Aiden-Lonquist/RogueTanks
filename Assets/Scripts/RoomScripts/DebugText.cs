using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using TMPro;

public class DebugText : MonoBehaviour
{
    public TMPro.TextMeshProUGUI roomCodeText;
    private GameObject RM;
    // Start is called before the first frame update
    void Start()
    {
        RM = GameObject.Find("RoomManager");
    }

    // Update is called once per frame
    void Update()
    {
        UpdateRoomCodeText();
    }

    private void UpdateRoomCodeText()
    {
        roomCodeText.text = "Room Code: " + RM.GetComponent<RoomManagement>().currentRoom.roomCode;
    }
}
