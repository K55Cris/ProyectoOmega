using UnityEngine;
using UnityEngine.UI;

public class Carta : MonoBehaviour
{
    //Id o nro de la carta.


    //Es el icono en la parte superior derecha, para CartaDigimon (battle type) puede ser A, B o C y para CartaOption (icon) Program, Item y Field.
    private string icon;
    //Imagen de la carta.
    private Image imagen;
    //El color del borde que rodea la carta, (Azul, Rojo o Dorado).
    private Color frameColor;

    public Carta Seleccionar()
    {
        return this;
    }

    //GaS




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
