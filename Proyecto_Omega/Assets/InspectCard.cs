using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DigiCartas;
using TMPro;
public class InspectCard : MonoBehaviour {

    public TextMeshProUGUI Nombre, Descripcion, Familia, Atributo, Tipo,Nivel, Habilidad,id;
    public TextMeshProUGUI PoA,PoB,PoC, TituloEvolucion;
    public Image ImageDigimon;
    public CanvasGroup canvas;
    public List<Image> Evoluciones;
    public GameObject Evolucion;
    public void OpenCard(DigiCarta Datos)
    {
        id.text = "St-" + Datos.id.ToString();
        Nombre.text = Datos.Nombre;
        Descripcion.text = Datos.descripcion;
        Familia.text = Datos.Familia;
        Atributo.text = Datos.Atributo;
        Nivel.text = Datos.Nivel;
        Tipo.text = Datos.Tipo;
        Habilidad.text = Datos.Habilidad;
        ImageDigimon.sprite = DataManager.instance.GetSprite(Datos.id);
        PoA.text = Datos.DanoAtaqueA.ToString();
        PoB.text = Datos.DanoAtaqueB.ToString();
        PoC.text = Datos.DanoAtaqueC.ToString();
        if (Datos.ListaRequerimientos.Count > 0)
        {
            // cargar Evoluciones
            Evolucion.gameObject.SetActive(true);
            TituloEvolucion.text = "Evoluciona de:";
            int Contador = 0;
            //Clear
            foreach (var item in Evoluciones)
            {
                item.gameObject.SetActive(false);
            }
            foreach (var item in Datos.ListaRequerimientos)
            {
                int posicion = item.IndexOf(" ");
                string Digimon = item.Substring(0, posicion);

                Debug.Log(Digimon);
                int ID = DataManager.instance.GetIDWithName(Digimon);
                Debug.Log(ID);
                if (ID!=0)
                {
                    Evoluciones[Contador].gameObject.SetActive(true);
                    Evoluciones[Contador].sprite = DataManager.instance.GetSprite(ID);
                }
               
                Contador++;
            }
        }
        else
        {
            TituloEvolucion.text = "No existen Pre-Evoluciones";
            //Apagar Evoluciones
            Evolucion.gameObject.SetActive(false);
        }
    }
    public void ClosePanel()
    {
        canvas.alpha = 0;
        canvas.blocksRaycasts = false;
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
