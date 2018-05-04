using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DigiCartas;
using UnityEngine.UI;
using UnityEngine.Events;
public class EditorCardBase : MonoBehaviour {

    public DigiCarta DatosDigimon;
    public Image ImageCarta, Contorno;
    public Text Cantida;
    public GameObject PanelPadre;
    public int num=1;
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
        ImageCarta.color = Color.white;
        Cantida.text = "0"+num;
    }
    public void ReducirCantidad()
    {
        num--;
        if (num==0)
        {
            ImageCarta.color = Color.gray; 
        }
        Cantida.text = "0" + num;
    }

    public void CantidadCartas(int cant)
    {
        num = cant;
        Cantida.text = "0" + num;

    }
    public void UsarCarta(UnityAction<DigiCarta> evento)
    {
        if (num == 1)
        {
            num = 0;
            ImageCarta.color = Color.gray;
            evento(DatosDigimon);
            Cantida.text = "00" ;
        }
        else
        {
            if (num > 0)
            {
                num--;
                evento(DatosDigimon);
                Cantida.text = "0" + num;
            }
        }
    }
    public void Seleccionar()
    {
        Contorno.color = Color.white;
        DeckManager.instance.CartaSeleccionada = this;
    }
}
