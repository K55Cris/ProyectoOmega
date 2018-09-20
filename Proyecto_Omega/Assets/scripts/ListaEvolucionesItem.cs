using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ListaEvolucionesItem : MonoBehaviour {
    public Text Texto;
	// Use this for initialization
	public void Crear(string Request)
    {
        Texto.text = Request;
    }
}
