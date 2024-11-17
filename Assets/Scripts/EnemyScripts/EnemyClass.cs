using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Enemy
{
    //these variables are case sensitive and must match the strings in the JSON.
    public string enemyCode; // unique enemy identifier (concat type, room, enemy count)
    public GameObject enemyType; // what type of enemy this is
    public float maxHealth; // default starting health
    public float curHealth; // remaining health of the enemy
    public float xPOS; // x position on room exit
    public float yPOS; // y position on room exit
    public bool isAlive; // should this enemy spawn on room entry

}