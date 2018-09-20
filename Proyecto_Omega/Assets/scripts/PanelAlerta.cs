using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class PanelAlerta : MonoBehaviour {

    public Transform Content;
    public GameObject DigiCarta;
    public GameObject ScrollPanel;
    public static PanelAlerta instance;
    private UnityAction<string> LoAceptar=null;
    private UnityAction<string> LoCancelar=null;
    public Text Mesaje;

    private void Awake()
    {
        instance = this;
    }

    public void Activar(UnityAction<string> Aceptar, UnityAction<string> Cancelar ,List<CartaDigimon> cartas, string mensaje)
    {
        LoAceptar = Aceptar;
        LoCancelar = Cancelar;
        Mesaje.text = mensaje;
        vaciar();
        foreach (var item in cartas)
        {
            GameObject Carta = Instantiate(DigiCarta, Content);
            Carta.GetComponent<SelectDigimon>().RecibirDatos(item.DatosDigimon, item.cardNumber, null);
        }
    }
    public void vaciar()
    {
        foreach (Transform item in Content)
        {
            Destroy(item.gameObject);
        }
    }
    public void BtAceptar()
    {
        if (LoAceptar != null)
        {
            LoAceptar.Invoke("Continuar de todos modos");
            LoAceptar = null;
            LoCancelar = null;
        }
        this.gameObject.SetActive(false);
    }
    public void BtCancelar()
    {
        if (LoCancelar != null)
        {
            LoCancelar.Invoke("Cancela cambio phase");
            LoAceptar = null;
            LoCancelar = null;
        }
        this.gameObject.SetActive(false);
    }

}
