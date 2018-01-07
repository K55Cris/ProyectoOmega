using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class CargarResources : MonoBehaviour {
    [TooltipAttribute("Deck al que esta vinculado la carga")]
    public GameObject deckAUtilizar;
    private Sprite[] arrayImage;
    private List<string> rookie = new List<string>();
    //Carga el deck desde un txt (aca deberia cargarlo desde la bdd)
    void CargarDeck()
    {
        //busca el deck desde un archivo
        StreamReader reader = new StreamReader(Application.dataPath+"/deck/Deck.txt");
        //aux
        string s;
        //carga todo mientras no sea nulo
        while ((s = reader.ReadLine()) != null)
        {
            //split para revisar sus demas componentes y ver si es un rookie
            if (s.Split(';')[1].Equals("-1"))
            {
                rookie.Add(s.Split(';')[0]);
            }
            //lo carga al deck
            deckAUtilizar.GetComponent<Deck>().GetDeck().Add(s.Split(';')[0]);
        }
    }
    //carga las imagenes a la lista de imagenes..
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
    //Este es el que se deberia usar (si estubiera completo) en este carga todos los datos al DataManager
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
