using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScrolling : MonoBehaviour
{

    public float Speed = 0.4f;
    public bool toUp = false;
    public bool toLeft = false;

    // Update is called once per frame
    void Update()
    {
        Vector2 offset = new Vector2(Time.time * Speed * (toLeft ? -1 : 1), Time.time * Speed * (toUp ? -1 : 1));
        GetComponent<Renderer>().material.mainTextureOffset = offset;
    }
}
