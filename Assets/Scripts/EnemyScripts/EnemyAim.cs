using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAim : MonoBehaviour
{
    public GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        AdjustRotation();
    }

    private void AdjustRotation()
    {
        Vector3 playerPOS = player.transform.position;
        //playerPOS = Camera.main.ScreenToWorldPoint(playerPOS);

        Vector2 direction = new Vector2(playerPOS.x - transform.position.x, playerPOS.y - transform.position.y);
        transform.up = direction;
    }
}
