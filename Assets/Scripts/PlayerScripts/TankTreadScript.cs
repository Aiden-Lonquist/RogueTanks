using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankTreadScript : MonoBehaviour
{
    public SpriteRenderer sr;
    private float alpha;
    // Start is called before the first frame update
    void Start()
    {
        alpha = 1;   
    }

    // Update is called once per frame
    void Update()
    {
        alpha -= 0.2f * Time.deltaTime;
        sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, alpha);

        if (alpha <= 0)
        {
            Destroy(gameObject);
        }
    }
}
