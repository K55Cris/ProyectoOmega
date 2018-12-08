using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DigiCartas;
using TMPro;
using System;

public class VentanaMoreInfo : MonoBehaviour {

    public static VentanaMoreInfo instance;
    public Image DigimonImage, Type;
    public DigiCarta Datos;
    public TextMeshProUGUI NombreAtaqueA, NombreAtaqueB, NombreAtaqueC, Habilidad, Evolucion1, Evolucion2, Evolucion3, NombreDigimon, Capacidad;
    public TextMeshProUGUI valorE,DañoA, DañoB, DañoC , Phase, atributo, Familia, Tipo,PerdidaVida1, PerdidaVida2,PerdidaVida3, PerdidaVida4, Coste;
    public CanvasGroup Panel;
    public GameObject DigimonCard, OptionCard;
    public ListaEvolucionesItem[] ItemEvoText;
    public Transform Contenido1, Contenido2;
    public GameObject PrefabEfecto;
    public bool Identico = false;

    private void Awake()
    {
        instance = this;
    }
    public void ShowDatos(DigiCarta DatosDigimon)
    {
        CancelInvoke("Cerrando");
        StopCoroutine(WhoIsPlayer1.ReduceAlpha(Panel));
        Panel.alpha = 1;
        DigimonImage.sprite = DataManager.instance.GetSprite(DatosDigimon.id);
        Datos = DatosDigimon;
        if (Datos.Capasidad == 0)
        {
            DigimonCard.SetActive(true);
            OptionCard.SetActive(false);
            Type.overrideSprite = DataManager.instance.GetSpriteForType(DatosDigimon);
            NombreAtaqueA.text = Datos.NombreAtaqueA;
            valorE.text = Datos.Nivel;
            NombreAtaqueB.text = Datos.NombreAtaqueB;
            NombreAtaqueC.text = Datos.NombreAtaqueC;
            DañoA.text = Datos.DanoAtaqueA.ToString();
            DañoB.text = Datos.DanoAtaqueB.ToString();
            DañoC.text = Datos.DanoAtaqueC.ToString();
            Habilidad.text = Datos.Habilidad;
            atributo.text = Datos.Atributo;
            Familia.text = Datos.Familia;
            Tipo.text = Datos.Tipo;
            PerdidaVida1.text = Datos.PerdidaVidaIII.ToString();
            PerdidaVida2.text = Datos.PerdidaVidaIV.ToString();
            PerdidaVida3.text = Datos.PerdidaVidaPerfect.ToString();
            PerdidaVida4.text = Datos.PerdidaVidaUltimate.ToString();
            int Contador = 0;
            foreach (var item in Datos.ListaRequerimientos)
            {
                switch (Contador)
                {
                    case 0:
                        Evolucion1.text = item;
                        break;
                    case 1:
                        Evolucion2.text = item;
                        break;
                    case 2:
                        Evolucion3.text = item;
                        break;
                }
                Contador++;
            }
            // Lista Efectos
            for (int i = 0; i < DatosDigimon.ListaEfectos.Count; i++)
            {
                if (Contenido1.GetChild(i))
                {
                    Contenido1.GetChild(i).gameObject.SetActive(true);
                }
                else
                {
                    GameObject.Instantiate(PrefabEfecto, Contenido1);
                }
            }
            int bandera = 0;
            foreach (Transform item in Contenido1)
            {
                if (item)
                {
                    if (DatosDigimon.ListaEfectos.Count > bandera)
                    {
                        item.GetComponent<ListaEvolucionesItem>().Crear(DatosDigimon.ListaEfectos[bandera]);
                    }
                    else
                    {
                        item.gameObject.SetActive(false);
                    }
                    bandera++;
                }
            }


        }
        else
        {
            DigimonCard.SetActive(false);
            OptionCard.SetActive(true);
            Phase.text = DatosDigimon.Limite;
            Capacidad.text = Datos.Capasidad.ToString();
            //ListaCategoria

            Type.overrideSprite = DataManager.instance.GetSpriteForType(DatosDigimon);
           
            Coste.text= string.Empty;
            foreach (var item in Datos.ListaCosto)
            {
                Coste.text = "\r\n";
                Coste.text += item;
            }
            // Lista Efectos
            for (int i = 0; i < DatosDigimon.ListaEfectos.Count; i++)
            {
                if (Contenido2.GetChild(i))
                {
                    Contenido2.GetChild(i).gameObject.SetActive(true);
                }
                else
                {
                    GameObject ExtraEfect = GameObject.Instantiate(PrefabEfecto, Contenido2);
                    ExtraEfect.GetComponent<LayoutElement>().preferredHeight = 85f;
                }
            }
            int bandera = 0;
            foreach (Transform item in Contenido2)
            {
                if (item)
                {
                    if (DatosDigimon.ListaEfectos.Count > bandera)
                    {
                        item.GetComponent<ListaEvolucionesItem>().Crear(DatosDigimon.ListaEfectos[bandera]);
                    }
                    else
                    {
                        item.gameObject.SetActive(false);
                    }
                    bandera++;
                }
            }
        }
        NombreDigimon.text = Datos.Nombre;
        Invoke("Cerrando", 4f);
        foreach (var item in ItemEvoText)
        {
            item.gameObject.SetActive(false);
        }
        for (int i = 0; i < DatosDigimon.ListaRequerimientos.Count; i++)
        {
            if (i <= 2)
            {
                    ItemEvoText[i].gameObject.SetActive(true);
                    ItemEvoText[i].Crear(DatosDigimon.ListaRequerimientos[i]);
            }
        }

    }

    public void Show(DigiCarta DatosDigimon)
    {
        if (Datos != DatosDigimon)
        {
            Identico = false;
            ShowDatos(DatosDigimon);
        }
        else
        {
            if (Identico)
            {
                Identico = false;
                ShowDatos(DatosDigimon);
            }
            else
            {
                StartCoroutine(Whaith(DatosDigimon));
            }
        }
    }
    public IEnumerator Whaith(DigiCarta DatosDigimon)
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForSeconds(1);
        Identico = true;

    }
    public void Cerrando()
    {
       StartCoroutine(WhoIsPlayer1.ReduceAlpha(Panel));
    }

}
