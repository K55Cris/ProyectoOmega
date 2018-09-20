using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DigiCartas;

public class InspectCard : MonoBehaviour {

    public Text Nombre;
    public Text Descripcion;
    public Text Familia;
    public Text Atributo;
    public Text Tipo;
    public Text Nivel;
    public Text Habilida;
    public Image ImageDigimon;
    public CanvasGroup canvas;

    public void OpenCard(DigiCarta Datos)
    {
        Nombre.text = Datos.Nombre;
        Descripcion.text = Datos.descripcion;
        Familia.text = Datos.Familia;
        Atributo.text = Datos.Atributo;
        Tipo.text = Datos.Tipo;
        Habilida.text = Datos.Habilidad;
        ImageDigimon.sprite = DataManager.instance.GetSprite(Datos.id);
    }
    public void ClosePanel()
    {
        DataManager.instance.FadeCanvas(canvas);
    }
    public void Open()
    {
        if (DeckManager.instance.CartaSeleccionada)
        {
            canvas.alpha = 1;
            canvas.blocksRaycasts = true;
            OpenCard(DeckManager.instance.CartaSeleccionada.DatosDigimon);
        }
    }
}
