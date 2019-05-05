using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DigimonModel : MonoBehaviour {

    public GameObject AtaqueParticula1;
    public Transform TargetAtack;
    public GameObject Cuerpo;
	// Use this for initialization
	void Start () {
		
	}

    public void AtacarParticula1()
    {
        Debug.Log("lel");
        GameObject flame= Instantiate(AtaqueParticula1, TargetAtack.transform.position, Quaternion.identity);
        Destroy(flame, 1f);
    }
    
}
