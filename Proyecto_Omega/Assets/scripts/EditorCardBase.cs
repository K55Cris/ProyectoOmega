using DigiCartas;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
public class EditorCardBase : MonoBehaviour
{

    public DigiCarta DatosDigimon;
    public Image ImageCarta, Contorno;
    public TextMeshProUGUI Cantidad;
    public GameObject PanelPadre;
    public int num = 1;
    private int IDCarta;

    public void RecibirDatos(DigiCarta Datos)
    {
        DatosDigimon = Datos;
        ImageCarta.sprite = DataManager.instance.GetSprite(DatosDigimon.id);
        Cantidad.gameObject.SetActive(false);
    }
    public void RecibirDatosDeck(DigiCarta Datos)
    {
        DatosDigimon = Datos;
        ImageCarta.sprite = DataManager.instance.GetSprite(DatosDigimon.id);
        Cantidad.gameObject.SetActive(true);
    }
    public void AumentarCantidad()
    {
        num++;
        ImageCarta.color = Color.white;
        Cantidad.text = "0" + num;
    }
    public void ReducirCantidad()
    {
        num--;
        if (num == 0)
        {
            ImageCarta.color = Color.gray;
        }
        Cantidad.text = "0" + num;
    }

    public void CantidadCartas(int cant)
    {
        num = cant;
        Cantidad.text = "0" + num;

    }
    public void UsarCarta(UnityAction<DigiCarta> evento)
    {
        if (num == 1)
        {
            num = 0;
            ImageCarta.color = Color.gray;
            evento(DatosDigimon);
            Cantidad.text = "00";
        }
        else
        {
            if (num > 0)
            {
                num--;
                evento(DatosDigimon);
                Cantidad.text = "0" + num;
            }
        }
    }
    public void Seleccionar()
    {
        Contorno.color = Color.white;
        DeckManager.instance.CartaSeleccionada = this;
    }
}
