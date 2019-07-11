using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class pointItem : MonoBehaviour {
    public Color Activo;
    public Color CDegradado;
    public Image Fondo;
    public Image FDegradado;
    public Text Cantidad;

    public void Apagar(Color apagado)
    {
        Fondo.color = apagado;
        if(Cantidad)
        Cantidad.color = new Color32(80, 80, 80, 255);
        FDegradado.color = new Color32(55, 55, 55, 210);
        if (Cantidad)
        Cantidad.GetComponent<Outline>().effectColor= new Color32(255,255 , 255, 120);
    }
    public void Prender()
    {
        Fondo.color = Activo;
        if(FDegradado)
        FDegradado.color = CDegradado;
        if (Cantidad)
        {
            Cantidad.color = new Color32(0, 164, 156, 255);
            Cantidad.GetComponent<Outline>().effectColor = new Color32(70, 237, 219, 255);
        }
    }
}
