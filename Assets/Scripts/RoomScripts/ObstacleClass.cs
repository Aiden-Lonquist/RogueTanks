using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Obstacle
{
    //these variables are case sensitive and must match the strings in the JSON.
    public GameObject obstacleType; // prefab for type of obstacle (rock, wall, water...)
    public string obstacleID;
    public float maxHealth; // how many hits the obstacle takes to destroy
    public float curHealth;
    public float xPOS; // x location in world space
    public float yPOS; // y location in world space
    public bool isActive; // true if the obstacle has not been destroyed
}