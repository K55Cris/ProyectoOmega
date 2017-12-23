using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CargarResources : MonoBehaviour {
    public GameObject canvasAUtilizar;
    public GameObject deckAUtilizar;
    List<string> Cargar()
    {
        List<string> deck = new List<string>();
        //carga el deck con las cartas lo hago a mano para probar
        for (int i = 0; i < 30; i++)
        {
            deck.Add("st-"+i);
        }
        return deck;
    }
	// Use this for initialization
	void Start () {
        //clase que carga todas las cartas de la partida en CNT
        //CNTdrag.arrayTextura = Resources.LoadAll<Texture>("Digimon");
        CNTdrag.arrayImage = Resources.LoadAll<Sprite>("Digimon");
        canvasAUtilizar.SetActive(true);
        CNTdrag.deck = Cargar();
        deckAUtilizar.SetActive(true);

    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
