using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScrolling : MonoBehaviour {

    public float Speed;
	
	// Update is called once per frame
	void Update () {
        Vector3 offset = new Vector3(Time.time * Speed, Time.time * Speed);
        GetComponent<Renderer>().material.mainTextureOffset = offset;
	}
}
