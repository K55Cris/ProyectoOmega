using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using DigiCartas;
public class SelectedDigimons : MonoBehaviour {



    public Image FondoNegro;
    public Transform Content;
    public GameObject DigiCarta;
    public GameObject ScrollPanel;
    public static SelectedDigimons instance;
    private void Awake()
    {
        instance = this;
        Terminar();
    }

    public void Activar(UnityAction<string> fase, List<CartaDigimon> cartas)
    {
        vaciar();
        FondoNegro.enabled = true;
        ScrollPanel.SetActive(true);
        foreach (var item in cartas)
        {
            GameObject Carta = Instantiate(DigiCarta, Content);
            Carta.GetComponent<SelectDigimon>().RecibirDatos(item.DatosDigimon,item.cardNumber,fase);
        }
    }
    public void vaciar()
    {
        foreach (Transform item in Content)
        {
            Destroy(item.gameObject);
        }
    }
    public void Terminar()
    {
        FondoNegro.enabled = false;
        ScrollPanel.SetActive(false);

    }
   
}
