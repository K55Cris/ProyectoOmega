using DigiCartas;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
public class DeckManager : MonoBehaviour {

    public static DeckManager instance;
   
    public List<DigiCarta> Deck;
    public List<DigiCarta> TemporalDeckEdition;

    public List<int> DeckDefecto;
    public List<int> DeckDefectoDisponible;

    public List<int> DeckInt;
    public GameObject CartaBase;
    public Transform BibliotecaContent, ViewEditorContent, DeckEditorContent;
    private float MaxCartas=0;
    public int Paginas = 0;
    public int nowPageB = 1;
    public Text TextPaginasBiblioteca, TextPaginasDeck;
    public Image[] puntosBiblioteca;
    public Image[] puntosDeck;
    public Color PuntoSelect;
    public Color PuntoDiseble;
    public GameObject Biblioteca,DeckEditor;
    public List<DIDCarta> IDCartasDisponibles;
    public Player PlayerDefault;
    public EditorCardBase CartaSeleccionada;
    public DeckItem[] SlotsDeck;
    public GameObject PanelAlertaBackDeck;
    private void Awake()
    {
        instance = this;
    }
    // Use this for initialization
    void Start () {
        // Cargar datos del Player
        Biblioteca.SetActive(false);
        ViewDeck();
        

    }

    public void AddDeck()
    {
        if (CartaSeleccionada)
        {
            if (CartaSeleccionada.num >= 1 && TemporalDeckEdition.Count<30)
            {
                bool CartaRepetida = false;

                foreach (var grouping in TemporalDeckEdition.GroupBy(t => t.id).Where(t => t.Count() != 1))
                {
                    if(grouping.Key==CartaSeleccionada.DatosDigimon.id&& grouping.Count() >= 3)
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
                    int slotVacio=-1;
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
            if (item.GetComponent<EditorCardBase>().DatosDigimon==Carta)
            {
                TemporalDeckEdition.Remove(Carta);
                item.GetComponent<EditorCardBase>().AumentarCantidad();
            }
        }
    }

   

    public void AbrirBiblioteca()
    {
        Biblioteca.SetActive(true);
        LoadAllCards();
    }
    public void OpenDeckEditor()
    {
        DeckEditor.SetActive(true);
        EditDeck();
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
            BackDontSave();
        }
        else
        {
            // Mostrar panel "Los mazos deben tener 30 cartas" 
            PanelAlertaBackDeck.SetActive(true);
        }
    }

    public void BackDontSave()
    {
        Deck = TemporalDeckEdition;
        DeckEditor.SetActive(false);
        Paginas = 0;
        nowPageB = 1;
        MaxCartas = 0;

        foreach (Transform item in DeckEditorContent)
        {
            Destroy(item.gameObject);
        }
        foreach (var item in SlotsDeck)
        {
            item.lleno=false;
        }
        TemporalDeckEdition = new List<DigiCarta>();
        ViewDeck();
    }
    public void GuardarDeck()
    {
        Deck = new List<DigiCarta>();
        foreach (var item in SlotsDeck)
        {
            Deck.Add(item._Datos);
        }
        SaveDeck();
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
    }

    private void OnEnable()
    {
       
    }

    public void LoadAllCards()
    {
     
        foreach (var item in DataManager.instance.TodasLasCartas)
        {
            GameObject NewCarta = Instantiate(CartaBase, BibliotecaContent);
            NewCarta.GetComponent<EditorCardBase>().RecibirDatos(item);
            MaxCartas++;
        }
        EntreHojas();
    }
    public void EntreHojas()
    {
        Paginas = Mathf.CeilToInt( MaxCartas/ 12);
        CargarListaCartas();
    }

    public void ViewDeck()
    {
        if (Deck.Count == 0)
        {
            GetDeck();
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
        yield return new  WaitForEndOfFrame();
        CartaSeleccionada = temporal;
        AddDeck();
        yield return new WaitForEndOfFrame();
    }
    public void EditDeck()
    {
        GetCardsAvaible();
        foreach (var item in PlayerManager.instance.IDCartasDisponibles)
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
        int arranque = (nowPageB -1) * 10;
        int CartasVisibles = 0;
        for (int i = 0; i < DeckEditorContent.childCount; i++)
        {
            if (i>= arranque)
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
        int arranque = (nowPageB-1) * 12;

        // apagamos todos
        
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
        TextPaginasBiblioteca.text = nowPageB + " de " + Paginas;
        puntosPaginas(puntosBiblioteca, nowPageB);
    }
	
	public void SaveDeck()
    {
        // guardar deck
        // revisar si el deck es valido para guardarse
        List<int> IdCartas= new List<int>();
        foreach (var item in Deck)
        {
            IdCartas.Add(item.id);
        }
        ListCard lD = new ListCard();
        lD.cards = IdCartas;
        string JSON = JsonUtility.ToJson(lD);
        Debug.Log(JSON);
        PlayerPrefs.SetString("MainDeck", JSON);
    }



    public void GetDeck()
    { 
        // cargar Deck
        string cartas = PlayerPrefs.GetString("MainDeck");
        if (!string.IsNullOrEmpty(cartas))
        {
            ListCard DeckT = JsonUtility.FromJson<ListCard>(cartas);
            DeckInt = DeckT.cards;
            foreach (var item in DeckT.cards)
            {
                Deck.Add(DataManager.instance.GetDigicarta(item));
            }
        }
        else
        {
            DeckInt = DeckDefecto;
            foreach (var item in DeckDefecto)
            {
                Deck.Add(DataManager.instance.GetDigicarta(item));
            }
        }
    }

    public void puntosPaginas(Image [] puntos, int nowPage)
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

    public void GetCardsAvaible()
    {
        if (PlayerPrefs.HasKey("Player"))
        {

        }
        else
        {
            // no existe un datos de jugador 
            Debug.Log("NO EXISTE JUGADOR");
            // cargar datos por defecto
            IDCartasDisponibles = new List<DIDCarta>();

            foreach (var item in DeckDefectoDisponible)
            {
                bool repetida = false;
                foreach (var item2 in IDCartasDisponibles)
                {
                    if (item2.ID == item)
                    {
                        item2.Cantidad++;
                        repetida = true;
                    }
                }
                if (!repetida)
                {
                    DIDCarta DIDC = new DIDCarta();
                    DIDC.Cantidad++;
                    DIDC.ID = item;
                    IDCartasDisponibles.Add(DIDC);
                }
            }
            PlayerManager.instance.IDCartasDisponibles = IDCartasDisponibles;
            PlayerManager.instance.Jugador = PlayerDefault;
            PlayerManager.instance.Jugador.IDCartasMazo = DeckDefecto;
        }
    }
}
