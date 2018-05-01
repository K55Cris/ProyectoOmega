using DigiCartas;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class DeckManager : MonoBehaviour {

    public static DeckManager instance;
   
    public List<DigiCarta> Deck;
    public List<int> DeckDefecto;
    public List<int> DeckInt;
    public GameObject CartaBase;
    public Transform BibliotecaContent, DeckEditorContent;
    private float MaxCartas=0;
    private int Paginas = 0;
    public int nowPage = 1;
    public Text TextPaginas;
    public Image[] puntos;
    public Color PuntoSelect;
    public Color PuntoDiseble;
    public GameObject Biblioteca;
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

    public void AbrirBiblioteca()
    {
        Biblioteca.SetActive(true);

        LoadAllCards();
    }

    public void BackViewDeck()
    {
        Biblioteca.SetActive(false);
        Paginas = 0;
        nowPage = 1;
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
        Debug.Log(MaxCartas / 12);
        Paginas = Mathf.CeilToInt( MaxCartas / 12);
        CargarListaCartas();
    }

    public void ViewDeck()
    {
        GetDeck();

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
                GameObject NewCarta = Instantiate(CartaBase, DeckEditorContent);
                NewCarta.GetComponent<EditorCardBase>().RecibirDatosDeck(DataManager.instance.GetDigicarta(item));
        }
        foreach (var item in ListaRepetidos)
        {
            foreach (Transform item2 in DeckEditorContent)
            {
                if (item2.GetComponent<EditorCardBase>().DatosDigimon.id == item)
                {
                    item2.GetComponent<EditorCardBase>().AumentarCantidad();
                }
            }
        }
    }


    public void Avanzar()
    {
        if (nowPage != Paginas)
        {
            nowPage++;
            CargarListaCartas();
        }
      
    }
    public void Regresar()
    {
        if (nowPage != 1)
        {
            nowPage--;
            CargarListaCartas();
        }
       
    }

    public void CargarListaCartas()
    {
        int arranque = (nowPage-1) * 12;

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
        TextPaginas.text = nowPage + " de " + Paginas;
        puntosPaginas();
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
        string JSON = JsonUtility.ToJson(IdCartas);
        PlayerPrefs.SetString("MainDeck", JSON);
    }

    public void GetDeck()
    { 
        // cargar Deck
        string cartas = PlayerPrefs.GetString("MainDeck");
        if (!string.IsNullOrEmpty(cartas))
        {
            List<int> DeckT = JsonUtility.FromJson<List<int>>(cartas);
            DeckInt = DeckT;
            foreach (var item in DeckT)
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

    public void puntosPaginas()
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
}
