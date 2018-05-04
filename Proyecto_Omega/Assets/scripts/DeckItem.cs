using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DigiCartas;
public class DeckItem : MonoBehaviour {
    public Text Nombre;
    public Text Nivel;
    public Text TipoBatalla;
    public DigiCarta _Datos;
    public bool lleno = false;


    public void Llenar(DigiCarta Datos)
    {
        Nombre.text = Datos.Nombre;
        Nivel.text = Datos.Nivel;
        TipoBatalla.text = Datos.TipoBatalla;
        if(Datos.TipoBatalla == "A")
        {
            TipoBatalla.color = Color.red;
        }
        else if (Datos.TipoBatalla == "B")
        {
            TipoBatalla.color = Color.green;
        }
        else if (Datos.TipoBatalla == "C")
        {
            TipoBatalla.color = Color.yellow;
        }
        lleno = true;
        _Datos = Datos;
    }
    public void Resetear()
    {
        Nombre.text = "Empty Slot";
        Nivel.text = "-";
        TipoBatalla.text = "-";
        lleno = false;
        DeckManager.instance.QuitDeck(_Datos);
    }
  
}
