using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class CargarResources : MonoBehaviour {
    public GameObject deckAUtilizar;
    private Sprite[] arrayImage;
    private List<string> rookie = new List<string>();
    void CargarDeck()
    {
        StreamReader reader = new StreamReader(Application.dataPath+"/deck/Deck.txt");
        string s;
        while ((s = reader.ReadLine()) != null)
        {
            if (s.Split(';')[1].Equals("-1"))
            {
                rookie.Add(s.Split(';')[0]);
            }
            deckAUtilizar.GetComponent<Deck>().GetDeck().Add(s.Split(';')[0]);
        }
    }
    void CargarImagenes()
    {
        arrayImage = Resources.LoadAll<Sprite>("Digimon");
        ScrollBaryCantidadDeCartas.instance.SetArrayRookie(new List<Sprite>());
        Deck.instance.SetArrayImage(new List<Sprite>());
        foreach (var imagen in arrayImage)
        {
            Deck.instance.GetArrayImage().Add(imagen);
            foreach (var nombre in rookie)
            {
                if (imagen.name.Equals(nombre))
                {
                    ScrollBaryCantidadDeCartas.instance.GetArrayRookie().Add(imagen);
                }
            }
        }
        ScrollBaryCantidadDeCartas.instance.CargarDatos();
        Deck.instance.FisherYates(Deck.instance.GetDeck());
    }
    void CargarListaCdartas()
    {

    }
    // Use this for initialization
    void Start () {
        CargarListaCdartas();
        CargarDeck();
        CargarImagenes();

    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
