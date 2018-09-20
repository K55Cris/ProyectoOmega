using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
public class intro : MonoBehaviour {
    public VideoPlayer Video;
	// Use this for initialization
	void Start () {
       
        Invoke("Continuar", (float)Video.clip.length);
	}
    private void Awake()
    {
        GetComponent<MeshRenderer>().enabled = true;
    }

    // Update is called once per frame
    void Update () {
		
	}
    public void Continuar()
    {
        if (gameObject)
        {
            gameObject.SetActive(false);
        }
    }
    public void OnMouseDown()
    {
        Continuar();
    }
}
