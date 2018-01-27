using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarkArea : MonoBehaviour {

    public List<Carta> Cartas;

    public List<Cartas> Vaciar()
    {

    }

    /*
     * Comentado tras el diseño del diagrama de clases.
     * 
    private List<string> dark = new List<string>();
    public static DarkArea instance;

    private void Awake()
    {
        instance = this;
    }

    public void Meter(string carta)
    {
        dark.Add(carta);
    }

    public List<string> Devolver()
    {
        print("se volvio a cargar el deck");
        List<string> devolver = new List<string>();
        foreach (var item in dark)
        {
            devolver.Add(item);
        }
        dark.Clear();
        return devolver;
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    */
}
