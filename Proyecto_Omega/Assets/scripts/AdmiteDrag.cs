using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdmiteDrag : MonoBehaviour {
    //variables
    public int tipo = 0;
    public Color color1 = Color.red;
    public Color color2 = Color.yellow;
    private float lerp;
    public float duration = 1.0f;
    private Material materialOriginal;
    public int nroSlot;

    private void OnMouseOver()
    {
        if (CNTdrag.clikeado) {
            CNTdrag.tipo = tipo;
            CNTdrag.objetoQuieto = gameObject;
            CNTdrag.slot = name;
            GetComponent<Renderer>().material = new Material(Shader.Find("Transparent/Diffuse"));
            color1.a = 0.5f;
            color2.a = 0.5f;
            GetComponent<Renderer>().material.color = Color.Lerp(color1, color2, lerp);
        }
    }
    
        
    private void OnTriggerEnter(Collider other)
    {
     /*   switch (nroSlot)
        {
            case 1:
                GameObject.Find("Option Slot 1").GetComponent<OptionSlot>().SetOcupado(true);
                break;
            case 2:
                GameObject.Find("Option Slot 2").GetComponent<OptionSlot>().SetOcupado(true);
                break;
            case 3:
                GameObject.Find("Option Slot 3").GetComponent<OptionSlot>().SetOcupado(true);
                break;
            default:
                break;
        }
        */
    }
    private void OnMouseExit()
    {
        CNTdrag.objetoQuieto = null;
        CNTdrag.tipo = -1;
        GetComponent<Renderer>().material = materialOriginal;
        
    }
    // Use this for initialization
    void Start () {
        materialOriginal = GetComponent<Renderer>().material;
    }
	
	// Update is called once per frame
	void Update () {
        lerp = Mathf.PingPong(Time.time, duration) / duration;
    }
}
