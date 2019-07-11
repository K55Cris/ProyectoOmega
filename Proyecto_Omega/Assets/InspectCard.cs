using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DigiCartas;
using TMPro;
public class InspectCard : MonoBehaviour {

    public TextMeshProUGUI Nombre, Descripcion, Familia, Atributo, Tipo,Nivel, Habilidad,id,Capacidad,FaseLimite,FaseActivacion;
    public TextMeshProUGUI PoA,PoB,PoC, TituloEvolucion;
    public Image ImageDigimon;
    public CanvasGroup canvas;
    public List<Image> Evoluciones;
    public GameObject Evolucion,ListaEvolucion,Ataques,DatosDigimon,DatosOption,Efectos,PrefabEfecto;
    public Transform Contenido2;

    public void OpenCard(DigiCarta Datos)
    {
        id.text = Datos.codigo;
        Nombre.text = Datos.Nombre;
        ImageDigimon.sprite = DataManager.instance.GetSprite(Datos.id);



        if (!Datos.IsSupport)
        {
            Ataques.SetActive(true);
            DatosDigimon.SetActive(true);
            DatosOption.SetActive(false);
            Efectos.SetActive(false);
            Evolucion.SetActive(true);
            Descripcion.text = Datos.descripcion;
            Familia.text = Datos.Familia;
            Atributo.text = Datos.Atributo;
            Nivel.text = Datos.Nivel;
            Tipo.text = Datos.Tipo;
            Habilidad.text = Datos.Habilidad;
           
            PoA.text = Datos.DanoAtaqueA.ToString();
            PoB.text = Datos.DanoAtaqueB.ToString();
            PoC.text = Datos.DanoAtaqueC.ToString();
            if (Datos.ListaRequerimientos.Count > 0 && Datos.ListaRequerimientos[0]!="")
            {
                // cargar Evoluciones
                ListaEvolucion.gameObject.SetActive(true);
                
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
                    if (ID != 0)
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
                ListaEvolucion.SetActive(false);
            }
        }
        else
        {
            // Option Cards
            Ataques.SetActive(false);
            Evolucion.SetActive(false);
            DatosDigimon.SetActive(false);
            DatosOption.SetActive(true);
            Efectos.SetActive(true);
            ListaEvolucion.gameObject.SetActive(false);

            FaseLimite.text = Datos.Limite;
            FaseActivacion.text = Datos.ListaActivacion[0];
            Capacidad.text = Datos.Capasidad.ToString();

            // Lista Efectos
            for (int i = 0; i < Datos.ListaEfectosMostrar.Count; i++)
            {
                if (Contenido2.GetChild(i))
                {
                    Contenido2.GetChild(i).gameObject.SetActive(true);
                }
            }
            int bandera = 0;
            foreach (Transform item in Contenido2)
            {
                if (item)
                {
                    if (Datos.ListaEfectosMostrar.Count > bandera)
                    {
                        item.GetComponent<ListaEvolucionesItem>().Crear(Datos.ListaEfectosMostrar[bandera]);
                    }
                    else
                    {
                        item.gameObject.SetActive(false);
                    }
                    bandera++;
                }
            }

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
