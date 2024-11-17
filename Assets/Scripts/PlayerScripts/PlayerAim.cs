using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAim : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        AdjustRotation();
    }

    private void AdjustRotation()
    {
        Vector3 mousePOS = Input.mousePosition;
        mousePOS = Camera.main.ScreenToWorldPoint(mousePOS);

        Vector2 direction = new Vector2(mousePOS.x - transform.position.x, mousePOS.y - transform.position.y);
        transform.up = direction;
    }
}
