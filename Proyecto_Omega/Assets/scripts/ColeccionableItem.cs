using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DigiCartas;
public class ColeccionableItem : MonoBehaviour
{
    public Image Imagen;
    public TextMeshProUGUI Nombre;
    
    public void CargarData(Coleccionables Datos)
    {
        Imagen.sprite = Datos.Image;
        Nombre.text = Datos.Nombre;
        Nombre.color = Datos.Rareza;
    }
}
