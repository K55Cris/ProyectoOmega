using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DigiCartas;
using UnityEngine.UI;

public class EditorCardBase : MonoBehaviour {

    public DigiCarta DatosDigimon;
    public Image ImageCarta;
    public Text Cantida;
    private int num=1;
    private int IDCarta;

    public void RecibirDatos(DigiCarta Datos)
    {
        DatosDigimon = Datos;
        ImageCarta.sprite = DataManager.instance.GetSprite(DatosDigimon.id);
        Cantida.gameObject.SetActive(false);
    }
    public void RecibirDatosDeck(DigiCarta Datos)
    {
        DatosDigimon = Datos;
        ImageCarta.sprite = DataManager.instance.GetSprite(DatosDigimon.id);
        Cantida.gameObject.SetActive(true);
    }
    public void AumentarCantidad()
    {
        num++;
        Cantida.text = "0"+num;
    }
}
