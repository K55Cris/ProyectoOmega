using DigiCartas;
using System.Collections.Generic;
using UnityEngine;
public class PlayerManager : MonoBehaviour
{

    public static PlayerManager instance;
    public SavePlayer Default;
    public SavePlayer Jugador;
    public Sound MusicaDuelo;
    public int IaPlaying;
    public IADecks DeckIA;
    public bool bienvenida = true;
    public Sprite ImagePhoto;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;

        }

        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(this.gameObject);

    }
    public void Start()
    {
        // conseguimos los datos
        GetData();
        MainMenu.instance.LoadName();
    }



    public void GetData()
    {
        // cargar Deck EN EL JUGADOR
        string cartas = PlayerPrefs.GetString("SavePlayer");
        if (cartas != "")
        {
            Jugador = JsonUtility.FromJson<SavePlayer>(cartas);
            // Datos cargados
            bienvenida = false;
        }
        else
        {
            Jugador = Default;
            // menu de nuevo jugador 
            bienvenida = true;
        }
        // Cargar Photo
        ImagePhoto = DataManager.instance.GetPerfilPhoto(Jugador.Photo);
    }
    public void SetNewColeccionable(Coleccionables Nuevo)
    {
        foreach (var item in Jugador.MisColeccionables)
        {
            if (item.ID == Nuevo.ID)
                return;
        }
        Jugador.MisColeccionables.Add(Nuevo);
        SavePlayer();
    }

    public List<int> GetDeck()
    {
        return Jugador.IDCartasMazo;
    }
    public List<Progreso> GetProgress()
    {
        return Jugador.Progresos;
    }
    public List<DIDCarta> GetCards()
    {
        return Jugador.IDCartasDisponibles;
    }
    public void SaveDeck(List<int> IDCartas)
    {
        // Guatdar Mazo
        Jugador.IDCartasMazo = IDCartas;
        SavePlayer();
    }
    public void SaveName(string Nombre)
    {
        // Guatdar Mazo
        Jugador.Nombre = Nombre;
        SavePlayer();
    }
    public void SaveCards(List<DIDCarta> IDCartasDisponibles)
    {
        // Guatdar Mazo
        Jugador.IDCartasDisponibles = IDCartasDisponibles;
        SavePlayer();
    }
    public void SavePlayer()
    {
        string JSON = JsonUtility.ToJson(Jugador);
        PlayerPrefs.SetString("SavePlayer", JSON);
    }

    public void NewCard(int ID)
    {
        bool bandera = false;
        foreach (var item in Jugador.IDCartasDisponibles)
        {
            if (item.ID == ID)
            {
                item.Cantidad++;
                bandera = true;
                break;
            }
        }
        if (!bandera)
        {
            DIDCarta NewID = new DIDCarta();
            NewID.ID = ID;
            NewID.Cantidad = 1;
            Jugador.IDCartasDisponibles.Add(NewID);
        }
        SavePlayer();
    }
    public void WinPlayer(int Puntos)
    {
        if (IaPlaying != 0)
        {
            Jugador.Progresos.Find(x => x.ID == IaPlaying).Victorias++;
            Jugador.Progresos.Find(x => x.ID == IaPlaying).Completo = true;
        }
        IaPlaying = 0;
        AddNivel(Puntos);
    }
    public void LosePlayer(int Puntos)
    {
        if (IaPlaying != 0)
        {
            Jugador.Progresos.Find(x => x.ID == IaPlaying).Derrotas++;
        }
        IaPlaying = 0;
        AddNivel(Puntos * -1);
    }

    public void NodoCerca(int ID)
    {
        foreach (var item in Jugador.Progresos)
        {
            if (item.ID == ID)
            {
                item.Cerca = true;
            }
        }
        SavePlayer();
    }
    public void AddNivel(int Cantidad)
    {
        Jugador.Nivel += Cantidad;
        if (Jugador.Nivel > 200)
            Jugador.Nivel = 200;
        if (Jugador.Nivel < 0)
            Jugador.Nivel = 0;
        SavePlayer();
    }
    public void ChangePhoto(string Foto)
    {
        Jugador.Photo = Foto;
        ImagePhoto = DataManager.instance.GetPerfilPhoto(Foto);
        SavePlayer();
    }
}
