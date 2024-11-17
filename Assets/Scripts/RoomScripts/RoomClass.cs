using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Room
{
    //these variables are case sensitive and must match the strings in the JSON.
    public string roomCode; // unique code defining this room
    public GameObject roomType; // type of room template used (circle, square...)
    //public Dictionary<string, string> doors; // dict of doors(key: 1,2,3,4) and their paths(value: room1,room2,room3)
    public string door1; // string containing room code of adjacent room for moving 
    public string door2;
    public string door3;
    public string door4;
    public List<Obstacle> obstacles; // list of all non-default things in the room
    public List<Enemy> enemies; // list of all enemies in the room;

}