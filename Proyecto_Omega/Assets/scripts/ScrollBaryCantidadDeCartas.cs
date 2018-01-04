using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollBaryCantidadDeCartas : MonoBehaviour {

    /*//cantidad de rookies que tiene en el deck
    public float w = 53;
    public float h = 93;
    public GameObject carta;
    private GameObject x;
    private float espacio = 0;
    public int cantidadDeCartas = 1;
    private Texture[] texturas;*/

    public GameObject prefCarta;
    public Transform contenedor;


    // Use this for initialization
    void Start () {
        /*texturas = Resources.LoadAll<Texture>("Digimon");

        //CNTdrag.claseCartas = this.transform.parent.parent.parent;
        //tamaño del content
        //Creo la cantidad de cartas
        espacio = (((w * cantidadDeCartas) /2) - (w * cantidadDeCartas)) + (w*3);
        print(this.transform.parent.right);
        for (int i = 1; i <= cantidadDeCartas; i++)//CNTdrag.arrayMaterial.Length; i++)
        {
            float x1 = this.transform.position.x;
            float y1 = this.transform.position.y;
            float z1 = this.transform.position.z;
            x = Instantiate(carta, new Vector3(x1+espacio,y1,z1) ,Quaternion.identity);
            x.transform.Rotate(90, 180, 0);
            x.gameObject.GetComponent<Renderer>().material.mainTexture = texturas[i-1];
            x.transform.SetParent(this.transform);
            espacio += w;
        }
        
       GetComponent<RectTransform>().sizeDelta = new Vector2((w* cantidadDeCartas) -(w*5),h);
       */
        foreach (var item in CNTdrag.arrayRookie)
        {
            GameObject cartas = Instantiate(prefCarta, contenedor);
            cartas.GetComponent<DetalleCarta>().Cargar(item);
        }
    }
	
	// Update is called once per frame
	void Update () {

    }
}
