using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class CargarResources : MonoBehaviour {
    public GameObject canvasAUtilizar;
    public GameObject deckAUtilizar;
    private Sprite[] arrayImage;
    private List<string> rookie = new List<string>();
    void Cargar()
    {
        StreamReader reader = new StreamReader(Application.dataPath+"/deck/Deck.txt");
        string s;
        while ((s = reader.ReadLine()) != null)
        {
            if (s.Split(';')[1].Equals("-1"))
            {
                rookie.Add(s.Split(';')[0]);
            }
            else
            {
                deckAUtilizar.GetComponent<Deck>().GetDeck().Add(s.Split(';')[0]);
            }
        }
    }
	// Use this for initialization
	void Start () {
        //clase que carga todas las cartas de la partida en CNT
        //CNTdrag.arrayTextura = Resources.LoadAll<Texture>("Digimon");
        Cargar();
        arrayImage = Resources.LoadAll<Sprite>("Digimon");
        CNTdrag.arrayImage = new List<Sprite>();
        foreach (var imagen in arrayImage)
        {
            foreach (var nombre in rookie)
            {
                if (imagen.name.Equals(nombre))
                {
                    CNTdrag.arrayImage.Add(imagen);
                }
            }
        }
        canvasAUtilizar.SetActive(true);
        deckAUtilizar.SetActive(true);

    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
