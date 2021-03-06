﻿using DigiCartas;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DeckManager : MonoBehaviour
{

    public static DeckManager instance;
    public Image PerfilPhoto;
    public List<DigiCarta> Deck;
    public List<DigiCarta> TemporalDeckEdition;
    public List<int> DeckInt;
    public GameObject CartaBase;
    public CanvasGroup CGViewDeck;
    public Transform BibliotecaContent, ViewEditorContent, DeckEditorContent, ColecionableContent;
    private float MaxCartas = 0;
    public int Paginas = 0;
    public int nowPageB = 1;
    public TextMeshProUGUI TextPaginasBiblioteca, TextPaginasDeck;
    public Image[] puntosBiblioteca;
    public Image[] puntosDeck;
    public Color PuntoSelect;
    public Color PuntoDiseble;
    public GameObject Biblioteca, DeckEditor;
    public List<DIDCarta> IDCartasDisponibles;
    public EditorCardBase CartaSeleccionada;
    public DeckItem[] SlotsDeck;
    public GameObject PanelAlertaBackDeck;
    public Transform customConten;
    public GameObject PrefabColecionable;
    public Image Photo, Tablero, BackTablero, BackCard;
    public TextMeshProUGUI TextPhoto, TextTablero, TextBackTablero, TextBackCard;
    private void Awake()
    {
        instance = this;
        Biblioteca.SetActive(false);
        ViewDeck();
    }
    // Use this for initialization

    public void BorarDatos()
    {
        PlayerPrefs.DeleteAll();
        SceneManager.LoadScene(0);
    }

    public void AddDeck()
    {
        if (CartaSeleccionada)
        {
            if (CartaSeleccionada.num >= 1 && TemporalDeckEdition.Count < 30)
            {
                bool CartaRepetida = false;

                foreach (var grouping in TemporalDeckEdition.GroupBy(t => t.id).Where(t => t.Count() != 1))
                {
                    if (grouping.Key == CartaSeleccionada.DatosDigimon.id && grouping.Count() >= 3)
                    {
                        Debug.Log(string.Format("'{0}' está repetido {1} veces.", grouping.Key, grouping.Count()));
                        CartaRepetida = true;
                    }

                }
                if (!CartaRepetida)
                {
                    TemporalDeckEdition.Add(CartaSeleccionada.DatosDigimon);
                    CartaSeleccionada.ReducirCantidad();
                    // buscamos un slot vacio 
                    int slotVacio = -1;
                    for (int i = 0; i < SlotsDeck.Length; i++)
                    {
                        if (!SlotsDeck[i].lleno)
                        {
                            slotVacio = i;
                            break;
                        }
                    }
                    if (slotVacio >= 0)
                    {
                        SlotsDeck[slotVacio].Llenar(CartaSeleccionada.DatosDigimon);
                        CartaSeleccionada = null;
                    }
                }
            }
            else
            {
                // no tienes cartas suficientes para agregar
            }
        }
    }
    public void QuitDeck(DigiCarta Carta)
    {
        foreach (Transform item in DeckEditorContent)
        {
            if (item.GetComponent<EditorCardBase>().DatosDigimon == Carta)
            {
                TemporalDeckEdition.Remove(Carta);
                item.GetComponent<EditorCardBase>().AumentarCantidad();
            }
        }
    }



    public void AbrirBiblioteca()
    {
        Biblioteca.SetActive(true);
        CGViewDeck.interactable = false;
        LoadAllCards();
    }
    public void OpenDeckEditor()
    {
        DeckEditor.SetActive(true);
        CGViewDeck.interactable = false;
        EditDeck();
    }

    public void OpenColec()
    {
        ColecionableContent.gameObject.SetActive(true);
        LoadData();
    }

    public void CloseViewDeck()
    {
        int cartasMaximos = 0;
        foreach (var item in SlotsDeck)
        {
            if (item.lleno)
                cartasMaximos++;
        }
        if (cartasMaximos == 30)
        {
            GuardarDeck();
            BackSave();
        }
        else
        {
            // Mostrar panel "Los mazos deben tener 30 cartas" 
            PanelAlertaBackDeck.SetActive(true);
        }
    }

    public void BackSave()
    {
        Deck = TemporalDeckEdition;
        DeckEditor.SetActive(false);
        Paginas = 0;
        nowPageB = 1;
        MaxCartas = 0;
        CartaSeleccionada = null;
        foreach (Transform item in DeckEditorContent)
        {
            Destroy(item.gameObject);
        }
        foreach (var item in SlotsDeck)
        {
            item.lleno = false;
        }
        TemporalDeckEdition = new List<DigiCarta>();
        ViewDeck();
    }
    public void BackDontSave()
    {
        DeckEditor.SetActive(false);
        Paginas = 0;
        nowPageB = 1;
        MaxCartas = 0;
        CartaSeleccionada = null;
        foreach (Transform item in DeckEditorContent)
        {
            Destroy(item.gameObject);
        }
        foreach (var item in SlotsDeck)
        {
            item.lleno = false;
        }
        TemporalDeckEdition = new List<DigiCarta>();
        ViewDeck();
    }

    public void GuardarDeck()
    {
        List<int> TemDeck = new List<int>();
        foreach (var item in SlotsDeck)
        {
            TemDeck.Add(item._Datos.id);
        }
        PlayerManager.instance.SaveDeck(TemDeck);
    }
    public void BackViewDeck()
    {
        Biblioteca.SetActive(false);
        Paginas = 0;
        nowPageB = 1;
        MaxCartas = 0;

        foreach (Transform item in BibliotecaContent)
        {
            Destroy(item.gameObject);
        }
        ViewDeck();
    }


    public void LoadAllCards()
    {

        foreach (var item in DataManager.instance.ColeccionDeCartas.DigiCartas)
        {
            if (item.id != 61)
            {
                GameObject NewCarta = Instantiate(CartaBase, BibliotecaContent);
                NewCarta.GetComponent<EditorCardBase>().RecibirDatos(item);
                MaxCartas++;
            }
        }

        foreach (var item in DataManager.instance.ColeccionDeCartas.CartaOption)
        {
            if (item.id != 61)
            {
                GameObject NewCarta = Instantiate(CartaBase, BibliotecaContent);
                NewCarta.GetComponent<EditorCardBase>().RecibirDatos(item);
                MaxCartas++;
            }
        }
        EntreHojas();
    }
    public void EntreHojas()
    {
        Paginas = Mathf.CeilToInt(MaxCartas / 12);
        CargarListaCartas();
    }

    public void ViewDeck()
    {
        CartaSeleccionada = null;

        if (Deck.Count == 0)
        {
            DeckInt = PlayerManager.instance.GetDeck();
            foreach (var item in DeckInt)
            {
                Deck.Add(DataManager.instance.GetDigicarta(item));
            }
        }
        else
        {
            DeckInt = new List<int>();
            foreach (var item in Deck)
            {
                DeckInt.Add(item.id);
            }
        }


        foreach (Transform item in ViewEditorContent)
        {
            Destroy(item.gameObject);
        }

        List<int> ListaTemporal = new List<int>();
        List<int> ListaRepetidos = new List<int>();
        // REVISAMOS SI HAY CARTAS REPETIDAS 
        foreach (var item in DeckInt)
        {
            if (!ListaTemporal.Contains(item))
            {
                ListaTemporal.Add(item);
            }
            else
            {
                ListaRepetidos.Add(item);
            }
        }
        foreach (var item in ListaTemporal)
        {
            GameObject NewCarta = Instantiate(CartaBase, ViewEditorContent);
            NewCarta.GetComponent<EditorCardBase>().RecibirDatosDeck(DataManager.instance.GetDigicarta(item));

        }
        foreach (var item in ListaRepetidos)
        {
            foreach (Transform item2 in ViewEditorContent)
            {
                if (item2.GetComponent<EditorCardBase>().DatosDigimon.id == item)
                {
                    item2.GetComponent<EditorCardBase>().AumentarCantidad();
                }
            }
        }

        CGViewDeck.interactable = true;

    }

    public EditorCardBase FindCardDeck(DigiCarta datos)
    {
        EditorCardBase temporal;
        foreach (Transform item in DeckEditorContent)
        {
            temporal = item.GetComponent<EditorCardBase>();
            if (item.GetComponent<EditorCardBase>().DatosDigimon == datos)
                return temporal;
        }
        return null;
    }


    public IEnumerator AddUIDeck(EditorCardBase temporal)
    {
        yield return new WaitForEndOfFrame();
        CartaSeleccionada = temporal;
        AddDeck();
        yield return new WaitForEndOfFrame();
    }
    public void EditDeck()
    {
        foreach (var item in PlayerManager.instance.Jugador.IDCartasDisponibles)
        {
            GameObject NewCarta = Instantiate(CartaBase, DeckEditorContent);
            NewCarta.GetComponent<EditorCardBase>().RecibirDatosDeck(DataManager.instance.GetDigicarta(item.ID));
            NewCarta.GetComponent<EditorCardBase>().CantidadCartas(item.Cantidad);
            MaxCartas++;
        }
        Paginas = Mathf.CeilToInt(MaxCartas / 10);
        CargarListaCartasDeck();

        foreach (DigiCarta item in Deck)
        {
            CartaSeleccionada = FindCardDeck(item);
            AddDeck();
        }

    }
    public void CargarListaCartasDeck()
    {
        int arranque = (nowPageB - 1) * 10;
        int CartasVisibles = 0;
        for (int i = 0; i < DeckEditorContent.childCount; i++)
        {
            if (i >= arranque)
            {
                if (CartasVisibles < 10)
                {
                    DeckEditorContent.GetChild(i).gameObject.SetActive(true);
                    CartasVisibles++;
                }
                else
                {
                    DeckEditorContent.GetChild(i).gameObject.SetActive(false);
                }
            }
            else
            {
                DeckEditorContent.GetChild(i).gameObject.SetActive(false);
            }
        }
        // Off Puntos 
        foreach (var item in puntosDeck)
        {
            item.gameObject.SetActive(false);
        }
        // cargar Puntos Dinamicos
        for (int i = 0; i < Paginas; i++)
        {
            puntosDeck[i].gameObject.SetActive(true);
        }

        TextPaginasDeck.text = nowPageB + " de " + Paginas;
        puntosPaginas(puntosDeck, nowPageB);



    }

    public void Avanzar()
    {
        if (nowPageB != Paginas)
        {
            nowPageB++;
            CargarListaCartas();
        }
    }
    public void Regresar()
    {
        if (nowPageB != 1)
        {
            nowPageB--;
            CargarListaCartas();
        }

    }
    public void AvanzarDeckEditor()
    {
        if (nowPageB != Paginas)
        {
            nowPageB++;
            CargarListaCartasDeck();
        }
    }
    public void RegresarDeckEditor()
    {
        if (nowPageB != 1)
        {
            nowPageB--;
            CargarListaCartasDeck();
        }

    }

    public void CargarListaCartas()
    {
        int arranque = (nowPageB - 1) * 12;

        // apagamos todos
        // Off Puntos 
        foreach (var item in puntosBiblioteca)
        {
            item.gameObject.SetActive(false);
        }

        int CartasVisibles = 0;
        foreach (Transform item in BibliotecaContent)
        {
            if (item.GetComponent<EditorCardBase>().DatosDigimon.id > arranque)
            {
                if (CartasVisibles < 12)
                {
                    item.gameObject.SetActive(true);
                    CartasVisibles++;
                }
                else
                {
                    item.gameObject.SetActive(false);
                }
            }
            else
            {
                item.gameObject.SetActive(false);
            }
        }
        // cargar Puntos Dinamicos
        for (int i = 0; i < Paginas; i++)
        {
            puntosBiblioteca[i].gameObject.SetActive(true);
        }
        TextPaginasBiblioteca.text = nowPageB + " de " + Paginas;
        puntosPaginas(puntosBiblioteca, nowPageB);


    }



    public void puntosPaginas(Image[] puntos, int nowPage)
    {
        int nowpoint = 1;
        foreach (var item in puntos)
        {
            if (nowPage == nowpoint)
            {
                item.GetComponent<Image>().color = PuntoSelect;

            }
            else
            {
                item.GetComponent<Image>().color = PuntoDiseble;
            }
            nowpoint++;
        }
    }
    public void GetIDs()
    {
        string DMazo = "";
        foreach (var item in DeckInt)
        {
            DMazo += item;
            DMazo += "\n";
        }
        Debug.Log(DMazo);
    }

    public void OpenCustom(int valor)
    {
        switch (valor)
        {
            case 0:
                llenarLista(ColeccionablesType.PerfilFoto);
                break;
            case 1:
                llenarLista(ColeccionablesType.Tablero);
                break;
            case 2:
                llenarLista(ColeccionablesType.FondoTablero);
                break;
            case 3:
                llenarLista(ColeccionablesType.Funda);
                break;
        }
    }

    public void llenarLista(ColeccionablesType Tipo)
    {
        List<Coleccionables> TempDatos = new List<Coleccionables>();
        foreach (var item in PlayerManager.instance.Jugador.MisColeccionables)
        {
            if (item.Tipo == Tipo)
            {
                TempDatos.Add(item);
            }
        }
        LlenarColecionables(TempDatos);
    }

    public void LlenarColecionables(List<Coleccionables> Datos)
    {
        int contador = 0;
        foreach (Transform item in customConten)
        {
            item.gameObject.SetActive(false);
        }
        foreach (var item in Datos)
        {
            if (contador < customConten.childCount)
            {
                //Rellenar Datos
                customConten.GetChild(contador).gameObject.SetActive(true);
                customConten.GetChild(contador).GetComponent<ColeccionableItem>().CargarData(item);
            }
            else
            {
                // crear nuevo contenedor
                GameObject NewContent = Instantiate(PrefabColecionable, customConten);
                NewContent.GetComponent<ColeccionableItem>().CargarData(item);
            }
            contador++;
        }
    }
    public void ChangeData(Coleccionables Datos)
    {

        switch (Datos.Tipo)
        {
            case ColeccionablesType.PerfilFoto:
                Photo.sprite = Datos.Image;
                PlayerManager.instance.ChangePhoto(Datos.Nombre);
                TextPhoto.text = Datos.Nombre;
                break;
            case ColeccionablesType.FondoTablero:
                BackTablero.sprite = Datos.Image;
                PlayerManager.instance.Jugador.FondoTablero = Datos.Nombre;
                TextBackTablero.text = Datos.Nombre;
                PlayerManager.instance.SavePlayer();
                break;
            case ColeccionablesType.Tablero:
                Tablero.sprite = Datos.Image;
                PlayerManager.instance.Jugador.Tablero = Datos.Nombre;
                TextTablero.text = Datos.Nombre;
                PlayerManager.instance.SavePlayer();
                break;
            case ColeccionablesType.Funda:
                BackCard.sprite = Datos.Image;
                PlayerManager.instance.Jugador.Funda = Datos.Nombre;
                TextBackCard.text = Datos.Nombre;
                PlayerManager.instance.SavePlayer();
                break;
            default:
                break;
        }
    }

    public void LoadData()
    {
        Photo.sprite = DataManager.instance.GetPerfilPhoto(PlayerManager.instance.Jugador.Photo);
        TextPhoto.text = PlayerManager.instance.Jugador.Photo;

        BackTablero.sprite = DataManager.instance.GetSpriteObjet(PlayerManager.instance.Jugador.FondoTablero, ColeccionablesType.FondoTablero);
        TextBackTablero.text = PlayerManager.instance.Jugador.FondoTablero;

        Tablero.sprite = DataManager.instance.GetSpriteObjet(PlayerManager.instance.Jugador.Tablero, ColeccionablesType.Tablero);
        TextTablero.text = PlayerManager.instance.Jugador.Tablero;

        BackCard.sprite = DataManager.instance.GetSpriteObjet(PlayerManager.instance.Jugador.Funda, ColeccionablesType.Funda);
        TextBackCard.text = PlayerManager.instance.Jugador.Funda;
    }

}
