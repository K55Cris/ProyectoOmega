using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
public class SelectedDigimons : MonoBehaviour
{


    public TextMeshProUGUI TITLE;
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

    public void Activar(UnityAction<string> fase, List<CartaDigimon> cartas, string Titulo)
    {
        vaciar();
        FondoNegro.enabled = true;
        ScrollPanel.SetActive(true);
        TITLE.text = Titulo;
        foreach (var item in cartas)
        {
            GameObject Carta = Instantiate(DigiCarta, Content);
            Carta.GetComponent<SelectDigimon>().RecibirDatos(item.DatosDigimon, item.cardNumber, fase);
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
