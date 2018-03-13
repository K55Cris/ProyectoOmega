using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DigiCartas;
using UnityEngine.Events;
public class SelectDigimon : MonoBehaviour {

    public DigiCarta DatosDigimon;
    public Image ImageCarta;
    public UnityAction<string> Localfase=null;
    private int IDCarta;
    public void RecibirDatos(DigiCarta Datos,int IDcard, UnityAction<string> fase)
    {
        DatosDigimon = Datos;
        ImageCarta.sprite = DataManager.instance.GetSprite(DatosDigimon.id);
        Localfase = fase;
        IDCarta = IDcard;
    }

    public void SelectedDigimon()
    {
        SelectedDigimons.instance.Terminar();
        Localfase(IDCarta.ToString());
    }


}
