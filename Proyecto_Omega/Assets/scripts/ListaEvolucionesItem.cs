using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class ListaEvolucionesItem : MonoBehaviour {
    public TextMeshProUGUI Texto;
	// Use this for initialization
	public void Crear(string Request)
    {
        Texto.text = Request;
    }
}
