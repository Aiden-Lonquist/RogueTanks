using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public string description;
    public string type;
    public Sprite powerUpImage;
    public float speed; // tread power up
    public float health; // body power up
    public float bulletSize; // gun power up
    public float fireRate; // gun power up
    public float bulletSpeed; // gun power up
    public float damage; // used for FMJ
    public string modifier; // all power ups

    public GameObject descriptionPopUp;
    private GameObject popUp;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnMouseEnter()
    {
        Debug.Log("Mouse Over: " + gameObject.name);
        popUp = Instantiate(descriptionPopUp, gameObject.transform.position, new Quaternion(0,0,0,0), gameObject.transform);
    }

    public void OnMouseExit()
    {
        Debug.Log("Mouse Off: " + gameObject.name);
        Destroy(popUp);
    }

    public (float, float, float, float, float, float, string) GetItemStats()
    {
        return (speed, health, bulletSize, fireRate, bulletSpeed, damage, modifier);
    }
}
