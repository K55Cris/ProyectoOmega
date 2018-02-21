using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Carta : MonoBehaviour {
    //Id o nro de la carta.
    private int cardNumber;
    //nombre de la carta.
    private string cardName;
    //Es el icono en la parte superior derecha, para CartaDigimon (battle type) puede ser A, B o C y para CartaOption (icon) Program, Item y Field.
    private string icon;
    //Descripcion de la carta.
    private string descripcion;
    //Imagen de la carta.
    private Image imagen;
    //El color del borde que rodea la carta, (Azul, Rojo o Dorado).
    private Color frameColor;

    public Carta Seleccionar()
    {
        return this;
    }

    //GaS
    public int CardNumber
    {
        get
        {
            return cardNumber;
        }

        set
        {
            cardNumber = value;
        }
    }

    public string CardName
    {
        get
        {
            return cardName;
        }

        set
        {
            cardName = value;
        }
    }

    public string Icon
    {
        get
        {
            return icon;
        }

        set
        {
            icon = value;
        }
    }

    public string Descripcion
    {
        get
        {
            return descripcion;
        }

        set
        {
            descripcion = value;
        }
    }

    public Image Imagen
    {
        get
        {
            return imagen;
        }

        set
        {
            imagen = value;
        }
    }

    public Color FrameColor
    {
        get
        {
            return frameColor;
        }

        set
        {
            frameColor = value;
        }
    }
}
